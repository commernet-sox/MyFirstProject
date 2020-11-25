using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace CPC.Http
{
    public class ApiClient : IApiClient
    {
        #region Members
        private readonly IHttpClientFactory _httpClientFactory;
        private static HttpClient _httpClient;
        private readonly ApiClientSettings _apiClientSettings;
        private static readonly object _lockToken = new object();
        private static readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private static readonly string _cachePrefix = "ams_oauth2_client:";
        #endregion

        #region Constructors
        public ApiClient(IHttpClientFactory httpClientFactory, ApiClientSettings settings)
        {
            _httpClientFactory = httpClientFactory;
            if (_httpClientFactory == null && _httpClient == null)
            {
                _httpClient = new HttpClient();
            }
            else if (_httpClientFactory != null)
            {
                _httpClient = _httpClientFactory.CreateClient();
            }
            if (settings.Headers != null && settings.Headers.Count > 0)
            {
                foreach (var pair in settings.Headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(pair.Key, pair.Value);
                }
            }
            _apiClientSettings = settings;
        }

        public ApiClient(ApiClientSettings settings, HttpClient httpClient)
        {
            if (httpClient == null && _httpClient == null)
            {
                _httpClient = new HttpClient();
            }
            else if (httpClient != null)
            {
                _httpClient = httpClient;
            }
            if (settings.Headers != null && settings.Headers.Count > 0)
            {
                foreach (var pair in settings.Headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(pair.Key, pair.Value);
                }
            }
            _apiClientSettings = settings;
        }
        #endregion

        public IClient CreateClient(string url, bool bearer = true)
        {
            var uri = UrlUtility.Combine(_apiClientSettings.GatewayUrl, url);
            var client = new Client(_httpClient, uri);
            if (bearer)
            {
                lock (_lockToken)
                {
                    var oc = GetToken();
                    if (oc.Code == ApiCode.Success)
                    {
                        client.SetBearerAuthentication(oc.Data);
                    }
                    else
                    {
                        throw new ApiException(oc);
                    }
                }
            }
            return client;
        }

        public MultipartFormDataContent GenFileContext(byte[] fileBuffer, string fileName, Dictionary<string, object> args)
        {
            var multipartFormDataContent = new MultipartFormDataContent();
            if (!args.IsNull())
            {
                foreach (var arg in args)
                {
                    multipartFormDataContent.Add(new StringContent(arg.Value.ConvertString()),
                        string.Format("\"{0}\"", arg.Key));
                }
            }

            multipartFormDataContent.Add(new ByteArrayContent(fileBuffer), "file", fileName);
            return multipartFormDataContent;
        }

        public Outcome<string> GetToken()
        {
            var cacheKey = string.Empty;
            switch (_apiClientSettings.GrantType)
            {
                case "password_credential":
                case "refresh_token":
                    if (!_apiClientSettings.UserName.IsNull())
                    {
                        cacheKey = _cachePrefix + _apiClientSettings.ClientId + "_" + _apiClientSettings.UserName;
                    }
                    break;
                case "client_credential":
                    cacheKey = _cachePrefix + _apiClientSettings.ClientId;
                    break;
                default:
                    break;
            }


            if (!cacheKey.IsNull() && _cache.TryGetValue(cacheKey, out var value))
            {
                return new Outcome<string>(value.ToString());
            }

            object arg;

            switch (_apiClientSettings.GrantType)
            {
                case "password_credential":
                    {
                        var refreshKey = cacheKey + "_refresh";
                        if (_cache.TryGetValue(refreshKey, out var refreshToken))
                        {
                            arg = new { _apiClientSettings.ClientId, _apiClientSettings.ClientSecret, _apiClientSettings.Scope, GrantType = "refresh_token", RefreshToken = refreshToken };
                        }
                        else
                        {
                            arg = new { _apiClientSettings.ClientId, _apiClientSettings.ClientSecret, _apiClientSettings.GrantType, _apiClientSettings.Scope, _apiClientSettings.UserName, _apiClientSettings.Password };
                        }
                        break;
                    }
                case "refresh_token":
                    arg = new { _apiClientSettings.ClientId, _apiClientSettings.ClientSecret, _apiClientSettings.GrantType, _apiClientSettings.Scope, _apiClientSettings.RefreshToken };
                    break;
                case "authorization_code":
                    arg = new { _apiClientSettings.ClientId, _apiClientSettings.ClientSecret, _apiClientSettings.GrantType, _apiClientSettings.Scope, _apiClientSettings.Code };
                    break;
                default:
                    arg = new { _apiClientSettings.ClientId, _apiClientSettings.ClientSecret, _apiClientSettings.GrantType, _apiClientSettings.Scope };
                    break;
            }

            var client = CreateClient(_apiClientSettings.OAuth2Address, false);

            var response = client.GetAsync().WithArguments(arg).AsResponse().Result;
            if (!response.IsSuccessStatusCode)
            {
                var msg = response.AsString().Result;
                var oc = msg.ToDataEx<Outcome>();
                if (oc != null)
                {
                    return new Outcome<string>(oc.Code, oc.Message);
                }
                else
                {
                    return new Outcome<string>(ApiCode.BadRequest, msg);
                }
            }

            var token = response.As<JwtToken>().Result;
            _cache.Set(cacheKey, token.Token, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(token.Expires)));

            if (!token.RefreshToken.IsNull() && !cacheKey.IsNull())
            {
                var refreshKey = cacheKey + "_refresh";
                _cache.Set(refreshKey, token.RefreshToken, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(30)));
            }
            return new Outcome<string>(token.Token);
        }
    }
}
