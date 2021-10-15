using Microsoft.AspNetCore.Mvc;
using PollyTest1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPC.DBCore;
using Microsoft.EntityFrameworkCore;

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
            TestApi testApi = new TestApi
            {
                CreateBy = "wangfeng",
                CreateTime = DateTime.Now,
                Name = Guid.NewGuid().ToString("N").Substring(10),
                Age = new Random().Next(10, 100),
            };

            _dbContext.TestApi.Add(testApi);
            //var dto = _dbContext.TestApi.FirstOrDefault();
            //dto.Name = Guid.NewGuid().ToString("N").Substring(10);
            //var org = _dbContext.TestApi.FirstOrDefault();
            //_dbContext.Entry(org).State = EntityState.Unchanged;
            //_dbContext.Entry(org).CurrentValues.SetValues(dto);
            //_dbContext.TestApi.Update(dto);
            _dbContext.SaveChanges(new Audit());
            var res = _dbContext.TestApi.ToList();
            return res;
        }
        /// <summary>
        /// 合并更新TestApi
        /// </summary>
        /// <returns></returns>
        [HttpPut("UpdateTestApis")]
        public TestApi UpdateTestApis(TestApi testApi)
        {
            //var org = _dbContext.TestApi.FirstOrDefault();
            //_dbContext.Entry(org).State = EntityState.Unchanged;
            //_dbContext.Entry(org).CurrentValues.SetValues(testApi);
            var data = _dbContext.TestApi.Update(testApi);
            _dbContext.SaveChanges(new Audit());
            var res = data.Entity;
            return res;
        }
        /// <summary>
        /// 更新TestApi加审计(savechanges二选一，一直接new audit(),二是在context重写savechanges)
        /// </summary>
        /// <param name="testApi"></param>
        /// <returns></returns>
        [HttpPut("UpdateTestApi")]
        public bool UpdateTestApi()
        {
            var org = _dbContext.TestApi.FirstOrDefault();
            org.Name= Guid.NewGuid().ToString("N").Substring(10);
            _dbContext.TestApi.Update(org);
            var res= _dbContext.SaveChanges();
            if (res>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
