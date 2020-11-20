using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoodApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1 from Good Api", "value2 from Good Api" };
        }
    }
}
