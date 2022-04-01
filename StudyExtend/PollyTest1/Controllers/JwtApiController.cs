using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PollyTest1.Controllers
{
    /// <summary>
    /// 测试授权token接口
    /// </summary>
    [ApiController]
    public class JwtApiController : ControllerBase
    {
        [HttpGet]
        [Route("api/value1")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value1" };
        }

        [HttpGet]
        [Route("api/value2")]
        [Authorize]
        public ActionResult<object> Get2()
        {
            var auth = HttpContext.AuthenticateAsync();
            var userName = auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            return new { username=userName };
        }
    }
}
