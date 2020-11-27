using CPC;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Application.Core;
using TestApi.Application.Services;
using TestData.DTO;
using TestData.RequestEntities;

namespace TestApi.Controllers
{
    /// <summary>
    /// 测试接口
    /// </summary>
    public class TestApiController:BaseController
    {
        private IOperate _ioperate;

        public TestApiController(IOperate operate)
        {
            _ioperate = operate;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetList")]
        public ActionResult<List<TestApiDTO>> GetList()
        {
            var service = GlobalContext.Resolve<TestApiService>();
            //return new List<TestApiDTO>() { new TestApiDTO { Name="王峰",Age=25} };
            return service.Query.ToList();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public ActionResult<bool> Add([FromQuery] TestData.RequestEntities.TestApiRequest dto)
        {
            var service = GlobalContext.Resolve<TestApiService>();
            TestData.DTO.TestApiDTO  testdto = dto.Map<TestData.DTO.TestApiDTO, TestData.RequestEntities.TestApiRequest>();
            var result = service.Add(testdto);
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

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public ActionResult<bool> Delete([FromQuery] TestApiRequest dto)
        {
            var service = GlobalContext.Resolve<TestApiService>();
            var oriDto = service.Query.Where(t => t.Id == dto.Id).FirstOrDefault();
            if (oriDto != null)
            {
                TestApiDTO testApiDTO = dto.Map<TestData.DTO.TestApiDTO, TestData.RequestEntities.TestApiRequest>();
                var result = service.Delete(testApiDTO);
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
