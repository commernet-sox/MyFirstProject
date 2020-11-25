using Data.IdentityService;
using Data.IdentityService.Model;
using AutoMapper;
using CPC;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace IdentityService.Controllers
{
    public class AppController : BaseController
    {
        private readonly AppService _app;

        public AppController(AppService app)
        {
            _app = app;
        }

        /// <summary>
        /// 根据ID获取APP的信息
        /// </summary>
        /// <param name="appKey"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<AuthAppDTO> Get(string appKey)
        {
            var mapper = GlobalContext.Resolve<IMapper>();
            var data = _app.Query.FirstOrDefault(t => t.AppKey == appKey);
            if (data == null)
            {
                return Custom(ApiCode.InvalidData, "appKey不正确");
            }
            return data;
        }

        /// <summary>
        /// 根据ID获取APP的信息
        /// </summary>
        /// <param name="appKey"></param>
        /// <returns></returns>
        [HttpGet("GetList")]
        public PageData<List<AuthAppDTO>> GetList([FromQuery]AppModel model)
        {
            var list = _app.Query;

            if (!string.IsNullOrWhiteSpace(model.AppKey))
            {
                list = list.Where(t => t.AppKey == model.AppKey);
            }
            if (!string.IsNullOrWhiteSpace(model.AppName))
            {
                list = list.Where(t => t.AppName == model.AppName);
            }
            list = list.ListPage(model.PageIndex, model.PageSize, out int totalCount);
            return new PageData<List<AuthAppDTO>>
            {
                Total = totalCount,
                Data = list.ToList()
            };
        }

        /// <summary>
        /// 创建APP的一条信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<AuthAppDTO> Post(AuthAppDTO data)
        {
            if (_app.Repository.Query.Any(t => t.AppKey == data.AppKey))
            {
                return Custom(ApiCode.DataDuplication, "appKey已存在");
            }

            _app.Add(data);

            return data;
        }

    }
}