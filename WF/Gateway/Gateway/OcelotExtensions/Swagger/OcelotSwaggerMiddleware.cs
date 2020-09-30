using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Ocelot.Configuration.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CPC;

namespace Gateway.OcelotExtensions.Swagger
{
    internal class OcelotSwaggerMiddleware
    {
        private static readonly Regex _pathTemplateRegex = new Regex(
            @"\{[^\}]+\}",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly IDistributedCache _cache;

        private readonly OcelotSwaggerOptions _options;

        private readonly IInternalConfigurationRepository _internalConfiguration;
        private readonly RequestDelegate _next;

        public OcelotSwaggerMiddleware(
            RequestDelegate next,
            IOptionsMonitor<OcelotSwaggerOptions> optionsAccessor,
            IInternalConfigurationRepository internalConfiguration,
            IDistributedCache cache)
        {
            _next = next;
            _options = optionsAccessor.CurrentValue;
            _internalConfiguration = internalConfiguration;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var path = httpContext.Request.Path.Value;

            string cacheEntry = null;
            string cacheKey = null;

            if (_options.Cache?.Enabled == true)
            {
                cacheKey = _options.Cache.KeyPrefix + StringUtility.UrlEncode(path);
                cacheEntry = await _cache.GetStringAsync(cacheKey);
            }

            if (cacheEntry != null)
            {
                CachedPathTemplate[] templates;

                using (var jsonReader = new JsonTextReader(new StringReader(cacheEntry)))
                {
                    templates = JsonConvert.DeserializeObject<CachedPathTemplate[]>(cacheEntry);
                }

                var newContent = await ReadContentAsync(httpContext);
                newContent = templates.Aggregate(
                    newContent,
                    (current, template) => current.Replace(
                        template.DownstreamPathTemplate,
                        template.UpstreamPathTemplate));
                await WriteContentAsync(httpContext, newContent);
            }
            else if (_options.SwaggerEndPoints.Exists(i => i.Url == path))
            {
                var ocelotConfig = _internalConfiguration.Get().Data;
                var matchedReRoute = (from i in ocelotConfig.Routes
                                      from j in i.DownstreamRoute
                                      where j.UpstreamPathTemplate.OriginalValue.Equals(
                                          path,
                                          StringComparison.OrdinalIgnoreCase)
                                      select j).ToList();
                if (matchedReRoute.Count > 0)
                {
                    var matchedHost = matchedReRoute.First().DownstreamAddresses.First();
                    var anotherReRoutes = (from i in ocelotConfig.Routes
                                           from j in i.DownstreamRoute
                                           where j.DownstreamAddresses.Exists(
                                               k => k.Host == matchedHost.Host && k.Port == matchedHost.Port)
                                           select j).ToList();

                    var templates = _options.Cache?.Enabled == true
                                        ? new List<CachedPathTemplate>(anotherReRoutes.Count)
                                        : null;

                    var newContent = await ReadContentAsync(httpContext);

                    foreach (var downstreamReRoute in anotherReRoutes)
                    {
                        var newDownstreamPathTemplate = _pathTemplateRegex.Replace(
                            downstreamReRoute.DownstreamPathTemplate.Value,
                            string.Empty);
                        var newUpstreamPathTemplate = _pathTemplateRegex.Replace(
                            downstreamReRoute.UpstreamPathTemplate.OriginalValue,
                            string.Empty);
                        templates?.Add(new CachedPathTemplate(newDownstreamPathTemplate, newUpstreamPathTemplate));
                        newContent = newContent.Replace(newDownstreamPathTemplate, newUpstreamPathTemplate);
                    }

                    if (_options.Cache?.Enabled == true)
                    {
                        await Task.WhenAll(
                            _cache.SetStringAsync(
                                cacheKey,
                                JsonConvert.SerializeObject(templates),
                                new DistributedCacheEntryOptions
                                {
                                    SlidingExpiration = TimeSpan.FromSeconds(
                                            _options.Cache.SlidingExpirationInSeconds)
                                }),
                            WriteContentAsync(httpContext, newContent));
                    }
                    else
                    {
                        await WriteContentAsync(httpContext, newContent);
                    }
                }
            }
            else
            {
                await _next(httpContext);
            }
        }

        private async Task<string> ReadContentAsync(HttpContext httpContext)
        {
            var existingBody = httpContext.Response.Body;
            using var newBody = new MemoryStream();
            // We set the response body to our stream so we can read after the chain of middlewares have been called.
            httpContext.Response.Body = newBody;

            await _next(httpContext);

            // Reset the body so nothing from the latter middlewares goes to the output.
            httpContext.Response.Body = existingBody;

            newBody.Seek(0, SeekOrigin.Begin);
            var newContent = await new StreamReader(newBody).ReadToEndAsync();

            return newContent;
        }

        private async Task WriteContentAsync(HttpContext httpContext, string content)
        {
            httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(content);
            await httpContext.Response.WriteAsync(content);
        }
    }
}
