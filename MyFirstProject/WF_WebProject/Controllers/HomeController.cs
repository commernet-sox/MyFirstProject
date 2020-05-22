using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WFWebProject.Models;

namespace WFWebProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext db;
        public HomeController(ILogger<HomeController> logger,DataContext dataContext)
        {
            _logger = logger;
            db = dataContext;
        }
        public async Task<IActionResult> Menu()
        {
            return View();
        }
        public async Task<IActionResult> Index()
        {
            var Tel = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(Tel))
                return RedirectToAction("Login","Home");
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> Login(Login model,string ReturnUrl=null)
        {
            try
            {
                ViewBag.ReturnUrl = ReturnUrl;
                //验证模型是否正确
                if (!ModelState.IsValid)
                {
                    return View();
                }
                DataTable dt = new DataTable();
                dt.Columns.Add("Tel",typeof(string));
                dt.Columns.Add("Pwd", typeof(string));
                dt.Columns.Add("roleID", typeof(string));
                if (db.User.Where(t=>t.UserName==model.Tel&&t.Pwd==model.Pwd).Count()>0)
                {
                    dt.Rows.Add("Tel",model.Tel);
                    dt.Rows.Add("Pwd", model.Pwd);
                    dt.Rows.Add("roleID", "123");
                }
                if (dt.Rows.Count >0)
                {
                    #region 登录认证，存入Cookie
                    //登录认证，存入Cookie
                    var claims = new List<Claim>(){
                                  new Claim(ClaimTypes.Name,model.Tel),new Claim("Pwd",model.Pwd),new Claim("roleID",dt.Rows[0]["roleID"].ToString())
                               };
                    //init the identity instances 
                    var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Customer"));
                    //signin 
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                        IsPersistent = false,
                        AllowRefresh = false
                    });
                    HttpContext.Session.SetString("User", model.Tel);
                    #endregion
                    //return Json(new { result = true, userName = dt.Rows[0]["Tel"], password = dt.Rows[0]["Pwd"], roleID = dt.Rows[0]["roleID"] });
                    if (ReturnUrl != null)
                    {
                        return Redirect(ReturnUrl);
                    }
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ModelState.AddModelError("", "用户名或密码输入错误，请重新输入");
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "登录失败" + ex.Message);
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
