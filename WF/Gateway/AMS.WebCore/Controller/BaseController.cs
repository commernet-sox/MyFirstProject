using CPC;
using CPC.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AMS.WebCore
{
    public class BaseController : Controller, IControllerData
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                //LogUtil.Write("OnActionExecuted调用出错", context.Exception);
                context.ExceptionHandled = true;
                context.Result = Json(new JsonResultData(ApiCode.SystemBusy, "系统繁忙"));
            }
            base.OnActionExecuted(context);
        }

        public JsonResult JsonToTabelPage(DataTable table)
        {
            if (table == null || table.Rows.Count < 1)
            {
                return Json(new TablePage { Total = 0, Rows = null });
            }
            var total = table.Rows.Count;
            if (table.Columns.Contains("counts"))
            {
                total = table.Rows[0]["counts"].ToString().ConvertInt32();
            }
            return Json(new TablePage { Total = total, Rows = table });
        }

        public JsonResult JsonNetResult(string result)
        {
            if (string.IsNullOrWhiteSpace(result))
            {
                return Json(string.Empty);
            }
            return Json(JsonConvert.DeserializeObject(result));
        }

        #region IControllerData

        public string UserId
        {
            get
            {
                var claim = User.Claims.FirstOrDefault(item => item.Type == "userId");
                if (claim != null)
                {
                    return claim.Value;
                }
                return string.Empty;
            }
        }

        public string UserPwd
        {
            get
            {
                var claim = User.Claims.FirstOrDefault(item => item.Type == "userPwd");
                if (claim != null)
                {
                    return claim.Value;
                }
                return string.Empty;
            }
        }

        public string Token
        {
            get
            {
                var claim = User.Claims.FirstOrDefault(item => item.Type == "token");
                if (claim != null)
                {
                    return claim.Value;
                }
                return string.Empty;
            }
        }
        #endregion

        #region http method
        protected virtual Outcome<string> GetToken(ApiClientSettings settings)
        {
            ApiClient client = new ApiClient(GlobalContext.Resolve<IHttpClientFactory>(), settings);
            return client.GetToken();
        }

        protected virtual IResponse Get(ApiClientSettings settings, string url, object args = null)
        {
            ApiClient client = new ApiClient(GlobalContext.Resolve<IHttpClientFactory>(), settings);
            return client.Get(url, args);
        }

        protected virtual IResponse Get(string url, object args = null)
        {
            var settings = AppSettingsUtil.GetApiClientSettings(UserId, UserPwd);
            ApiClient client = new ApiClient(GlobalContext.Resolve<IHttpClientFactory>(), settings);
            return client.Get(url, args);
        }

        protected virtual Task<IResponse> GetAsync(string url, object args = null)
        {
            var settings = AppSettingsUtil.GetApiClientSettings(UserId, UserPwd);
            ApiClient client = new ApiClient(GlobalContext.Resolve<IHttpClientFactory>(), settings);
            return client.GetAsync(url, args);
        }

        protected virtual IResponse Post<T>(string url, T body)
        {
            var settings = AppSettingsUtil.GetApiClientSettings(UserId, UserPwd);
            ApiClient client = new ApiClient(GlobalContext.Resolve<IHttpClientFactory>(), settings);
            return client.Post<T>(url, body);
        }

        protected virtual Task<IResponse> PostAsync<T>(string url, T body)
        {
            var settings = AppSettingsUtil.GetApiClientSettings(UserId, UserPwd);
            ApiClient client = new ApiClient(GlobalContext.Resolve<IHttpClientFactory>(), settings);
            return client.PostAsync<T>(url, body);
        }

        #endregion


    }
}
