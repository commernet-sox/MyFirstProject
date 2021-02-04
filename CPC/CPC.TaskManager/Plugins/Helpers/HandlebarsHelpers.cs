﻿using CPC.TaskManager.Plugins.TypeHandlers;
using HandlebarsDotNet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using static CPC.TaskManager.Plugins.Controllers.PageControllerBase;

namespace CPC.TaskManager.Plugins
{
    internal class HandlebarsHelpers
    {
        private readonly Services _services;

        public HandlebarsHelpers(Services services) => _services = services;

        public static void Register(Services services) => new HandlebarsHelpers(services).RegisterInternal();

        private void RegisterInternal()
        {
            var h = _services.Handlebars;

            h.RegisterHelper("Upper", (o, c, a) => o.Write(a[0].ToString().ToUpper()));
            h.RegisterHelper("Lower", (o, c, a) => o.Write(a[0].ToString().ToLower()));
            h.RegisterHelper("LocalTimeZoneInfoId", (o, c, a) => o.Write(TimeZoneInfo.Local.Id));
            h.RegisterHelper("SystemTimeZonesJson", (o, c, a) => Json(o, c, TimeZoneInfo.GetSystemTimeZones().ToDictionary()));
            h.RegisterHelper("DefaultDateFormat", (o, c, a) => o.Write(DateTimeSettings.DefaultDateFormat));
            h.RegisterHelper("DefaultTimeFormat", (o, c, a) => o.Write(DateTimeSettings.DefaultTimeFormat));
            h.RegisterHelper("DoLayout", (o, c, a) => c.Layout());
            h.RegisterHelper("SerializeTypeHandler", (o, c, a) => o.WriteSafeString(((Services)a[0]).TypeHandlers.Serialize((TypeHandlerBase)c)));
            h.RegisterHelper("Disabled", (o, c, a) => { if (IsTrue(a[0])) { o.Write("disabled"); } });
            h.RegisterHelper("Checked", (o, c, a) => { if (IsTrue(a[0])) { o.Write("checked"); } });
            h.RegisterHelper("nvl", (o, c, a) => o.Write(a[a[0] == null ? 1 : 0]));
            h.RegisterHelper("not", (o, c, a) => o.Write(IsTrue(a[0]) ? "False" : "True"));

            h.RegisterHelper(nameof(BaseUrl), (o, c, a) => o.WriteSafeString(BaseUrl));
            h.RegisterHelper(nameof(MenuItemActionLink), MenuItemActionLink);
            h.RegisterHelper(nameof(RenderJobDataMapValue), RenderJobDataMapValue);
            h.RegisterHelper(nameof(ViewBag), ViewBag);
            h.RegisterHelper(nameof(ActionUrl), ActionUrl);
            h.RegisterHelper(nameof(Json), Json);
            h.RegisterHelper(nameof(Selected), Selected);
            h.RegisterHelper(nameof(IsType), IsType);
            h.RegisterHelper(nameof(EachPair), EachPair);
            h.RegisterHelper(nameof(EachItems), EachItems);
            h.RegisterHelper(nameof(ToBase64), ToBase64);
            h.RegisterHelper(nameof(Footer), Footer);
            h.RegisterHelper(nameof(Version), Version);
            h.RegisterHelper(nameof(Logo), Logo);
            h.RegisterHelper(nameof(ProductName), ProductName);
        }

        private static bool IsTrue(object value) => value?.ToString()?.Equals("true", StringComparison.OrdinalIgnoreCase) == true;

        private string HtmlEncode(object value) => _services.ViewEngine.Encode(value);

        private string UrlEncode(string value) => HttpUtility.UrlEncode(value);

        private string BaseUrl
        {
            get
            {
                var url = _services.Options.VirtualPathRoot;
                if (!url.EndsWith("/"))
                {
                    url += "/";
                }

                return url;
            }
        }

        private string AddQueryString(string uri, IEnumerable<KeyValuePair<string, object>> queryString)
        {
            if (queryString == null)
            {
                return uri;
            }

            var anchorIndex = uri.IndexOf('#');
            var uriToBeAppended = uri;
            var anchorText = "";
            // If there is an anchor, then the query string must be inserted before its first occurence.
            if (anchorIndex != -1)
            {
                anchorText = uri.Substring(anchorIndex);
                uriToBeAppended = uri.Substring(0, anchorIndex);
            }

            var queryIndex = uriToBeAppended.IndexOf('?');
            var hasQuery = queryIndex != -1;

            var sb = new StringBuilder();
            sb.Append(uriToBeAppended);

            foreach (var parameter in queryString)
            {
                sb.Append(hasQuery ? '&' : '?');
                sb.Append(UrlEncode(parameter.Key));
                sb.Append('=');
                sb.Append(UrlEncode(string.Format(CultureInfo.InvariantCulture, "{0}", parameter.Value)));
                hasQuery = true;
            }

            sb.Append(anchorText);
            return sb.ToString();
        }

        private void ViewBag(TextWriter output, dynamic context, params object[] arguments)
        {
            var dict = (IDictionary<string, object>)arguments[0];
            var viewBag = (IDictionary<string, object>)context.ViewBag;

            foreach (var pair in dict)
            {
                viewBag[pair.Key] = pair.Value;
            }
        }

        private void MenuItemActionLink(TextWriter output, dynamic context, params object[] arguments)
        {
            var dict = arguments[0] as IDictionary<string, object> ?? new Dictionary<string, object>() { ["controller"] = arguments[0] };

            var classes = "item";
            if (dict["controller"].Equals(context.ControllerName))
            {
                classes += " active";
            }

            var url = BaseUrl + dict["controller"];
            var title = HtmlEncode(dict.GetValue("title", dict["controller"]));

            output.WriteSafeString($@"<a href=""{url}"" class=""{classes}"">{title}</a>");
        }

        private void ActionUrl(TextWriter output, dynamic context, params object[] arguments)
        {
            if (arguments.Length < 1 || arguments.Length > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(arguments));
            }

            IDictionary<string, object> routeValues = null;
            string controller = null;
            var action = (arguments[0] as Page)?.ActionName ?? (string)arguments[0];

            if (arguments.Length >= 2) // [actionName, controllerName/routeValues ]
            {
                if (arguments[1] is IDictionary<string, object> r)
                {
                    routeValues = r;
                }
                else if (arguments[1] is string s)
                {
                    controller = s;
                }
                else if (arguments[1] is Page v)
                {
                    controller = v.ControllerName;
                }
                else
                {
                    throw new Exception("ActionUrl: Invalid parameter 1");
                }
            }
            if (arguments.Length == 3) // [actionName, controllerName, routeValues]
            {
                routeValues = (IDictionary<string, object>)arguments[2];
            }

            if (controller == null)
            {
                controller = context.ControllerName;
            }

            var url = BaseUrl + controller;

            if (!string.IsNullOrEmpty(action))
            {
                url += "/" + action;
            }

            output.WriteSafeString(AddQueryString(url, routeValues));
        }

        private void Selected(TextWriter output, dynamic context, params object[] arguments)
        {
            string selected;
            if (arguments.Length >= 2)
            {
                selected = arguments[1]?.ToString();
            }
            else
            {
                selected = context["selected"].ToString();
            }

            if (((string)arguments[0]).Equals(selected, StringComparison.InvariantCultureIgnoreCase))
            {
                output.Write("selected");
            }
        }

        private void Json(TextWriter output, dynamic context, params object[] arguments) => output.WriteSafeString(Newtonsoft.Json.JsonConvert.SerializeObject(arguments[0]));

        private void RenderJobDataMapValue(TextWriter output, dynamic context, params object[] arguments)
        {
            var item = (JobDataMapItem)arguments[1];
            output.WriteSafeString(item.SelectedType.RenderView((Services)arguments[0], item.Value));
        }

        private void IsType(TextWriter writer, HelperOptions options, dynamic context, params object[] arguments)
        {
            var strType = (string)arguments[1];

            Type[] expectedType;
            switch (strType)
            {
                case "IEnumerable<string>":
                    expectedType = new[] { typeof(IEnumerable<string>) };
                    break;
                case "IEnumerable<KeyValuePair<string, string>>":
                    expectedType = new[] { typeof(IEnumerable<KeyValuePair<string, string>>) };
                    break;
                default:
                    throw new ArgumentException("Invalid type: " + strType);
            }

            var t = arguments[0]?.GetType();

            if (expectedType.Any(x => x.IsAssignableFrom(t)))
            {
                options.Template(writer, (object)context);
            }
            else
            {
                options.Inverse(writer, (object)context);
            }
        }

        private void EachPair(TextWriter writer, HelperOptions options, dynamic context, params object[] arguments)
        {
            void OutputElements<T>()
            {
                if (arguments[0] is IEnumerable<T> pairs)
                {
                    foreach (var item in pairs)
                    {
                        options.Template(writer, item);
                    }
                }
            }

            OutputElements<KeyValuePair<string, string>>();
            OutputElements<KeyValuePair<string, object>>();
        }

        private void EachItems(TextWriter writer, HelperOptions options, dynamic context, params object[] arguments) => EachPair(writer, options, context, ((dynamic)arguments[0]).GetItems());

        private void ToBase64(TextWriter output, dynamic context, params object[] arguments)
        {
            var bytes = (byte[])arguments[0];

            if (bytes != null)
            {
                output.Write(Convert.ToBase64String(bytes));
            }
        }

        private void Footer(TextWriter writer, HelperOptions options, dynamic context, params object[] arguments)
        {
            IDictionary<string, object> viewBag = context.ViewBag;

            if (viewBag.TryGetValue("ShowFooter", out var show) && (bool)show == true)
            {
                options.Template(writer, (object)context);
            }
        }

        private void Version(TextWriter output, dynamic context, params object[] arguments)
        {
            var v = GetType().Assembly.GetCustomAttributes<AssemblyInformationalVersionAttribute>().FirstOrDefault();
            output.Write(v.InformationalVersion);
        }

        private void Logo(TextWriter output, dynamic context, params object[] arguments) => output.Write(_services.Options.Logo);

        private void ProductName(TextWriter output, dynamic context, params object[] arguments) => output.Write(_services.Options.ProductName);
    }
}