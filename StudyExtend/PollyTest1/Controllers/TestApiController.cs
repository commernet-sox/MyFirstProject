using Microsoft.AspNetCore.Mvc;
using PollyTest1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollyTest1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestApiController : ControllerBase
    {
        private readonly PollyTestDbContext _dbContext;
        public TestApiController(PollyTestDbContext pollyTestDbContext)
        {
            _dbContext = pollyTestDbContext;
        }
        /// <summary>
        /// 获取TestApi
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTestApis")]
        public List<TestApi> GetTestApis()
        {
            var res = _dbContext.TestApi.ToList();
            return res;
        }
    }
}
