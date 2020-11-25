using Data.IdentityService;
using Data.IdentityService.Model;
using AMS.WebCore;
using CPC;
using CPC.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;

namespace AMS.Web.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController()
        {
        }

        public IActionResult Index()
        {
            var checkLogin = User.Claims.FirstOrDefault(item => item.Type.ToLower() == "loginresult" &&
            item.Value.ToLower() == "success");
            if (checkLogin != null)
            {
                return Redirect("/portal/portal");
            }
            return View("Login");
        }

        [HttpPost]
        public IActionResult CheckLogin(LoginModel login)
        {
            if (string.IsNullOrWhiteSpace(login.UserId) || string.IsNullOrWhiteSpace(login.UserPwd))
            {
                return Json(new JsonResultData(ApiCode.DataMissing, "参数缺失"));
            }
            var setting = AppSettingsUtil.GetApiClientSettings(login.UserId, login.UserPwd);
            var result = GetToken(setting);
            if (result.Code == ApiCode.Success)
            {
                var dto = Get(setting, "/os/1.0/user/GetEmployee").Result<SysEmployeeDTO>();
                var userName = dto.Data.EmployeeName;
                //身份信息
                var claims = new List<Claim>()
                {
                    new Claim("userId",login.UserId),
                    new Claim("userPwd",login.UserPwd),
                    new Claim("userName",userName),
                    new Claim("userShortName",userName.Length>2?userName.Substring(userName.Length-2):userName),
                    new Claim("loginresult", "success"),
                    new Claim("token", result.Message)
                };
                //身份证
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                //证件所有者
                var claimsPrinciple = new ClaimsPrincipal(claimsIdentity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrinciple, new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddHours(2)
                    });
                return Json(new JsonResultData(ApiCode.Success, "success", claims));
            }
            else
            {
                return Json(new JsonResultData(ApiCode.InvalidData, "failure", result.Message));
            }

        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string type = null,string redirect=null,string appid=null)
        {
            if (type == "logout")
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            if (!redirect.IsNull())
            {
                ViewData["redirect"] = redirect;
            }
            return View();
        }

        public OAuth2CodeResponse GetAuthCode(string userId)
        {
            var result = Get("/os/1.0/oauth2/code", new { userId = userId }).As<OAuth2CodeResponse>().Result;
            return result;
        }

    }

    public class ClaimData
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
