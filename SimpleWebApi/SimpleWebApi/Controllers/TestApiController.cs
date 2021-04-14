using CPC;
using CPC.Redis;
using Microsoft.AspNetCore.Mvc;
using SimpleWebApi.Application.Cache.Attributes;
using SimpleWebApi.Application.Core;
using SimpleWebApi.Application.Service;
using SimpleWebApi.Data.DTO;
using SimpleWebApi.Data.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebApi.Controllers
{
    /// <summary>
    /// 测试接口
    /// </summary>
    public class TestApiController : BaseController
    {
        private IOperate _ioperate;
        public TestApiController(IOperate operate)
        {
            _ioperate = operate;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetList")]
        public ActionResult<List<TestApiDTO>> GetList()
        {
            var service = GlobalContext.Resolve<TestApiService>();
            ////return new List<TestApiDTO>() { new TestApiDTO { Name="王峰",Age=25} };
            //return service.Query.ToList();
            //var redisClient = GlobalContext.Resolve<RedisClient>();
            return service.GetTestApis();
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public ActionResult<bool> Add(TestApiAddRequest request)
        {
            var service = GlobalContext.Resolve<TestApiService>();
            TestApiDTO testApiDTO = request.Map<TestApiDTO, TestApiAddRequest>();
            var result = service.Add(testApiDTO);
            if (!result.IsNull())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public ActionResult<bool> Update([FromQuery] TestApiRequest dto)
        {
            var service = GlobalContext.Resolve<TestApiService>();
            var oriDto = service.Query.Where(t => t.Id == dto.Id).FirstOrDefault();
            if (oriDto != null)
            {
                oriDto.Name = dto.Name;
                _ioperate.Initialize(oriDto);
                var result = service.Update(oriDto);
                if (result)
                {
                    return Custom(ApiCode.Success,"true");
                }
                else
                {
                    return Custom(ApiCode.InvalidData,"false");
                }
            }
            else
            {
                return Custom(ApiCode.InvalidData, "false");
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Obsolete]
        [HttpDelete("Delete")]
        public ActionResult<bool> Delete(TestApiDTO dto)
        {
            var service = GlobalContext.Resolve<TestApiService>();
            var oriDto = service.Query.Where(t => t.Id == dto.Id).FirstOrDefault();
            if (oriDto != null)
            {
                var result = service.Delete(dto);
                if (result)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
