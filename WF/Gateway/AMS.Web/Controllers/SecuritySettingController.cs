using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.IdentityService.DTO;
using AMS.WebCore;
using CPC;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Web.Controllers
{
    public class SecuritySettingController : BaseController
    {
        public IActionResult SecuritySetting()
        {
            return View();
        }
        
        public Outcome Update(SysSecuritySettingDTO dto)
        {
            var result = Post("/os/1.0/user/PwdUpdate", dto).As<Outcome>().Result;
            return result;
        }
    }
}
