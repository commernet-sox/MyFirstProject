using System;
using System.Collections.Generic;
using System.Linq;
using Data.IdentityService;
using Data.IdentityService.Model;
using CPC;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    public class OplogController : BaseController
    {
        private readonly SysOplogService _service;

        public OplogController(SysOplogService service)
        {
            _service = service;
        }

        [HttpGet("GetList")]
        public PageData<List<SysOplogDTO>> GetList([FromQuery]OplogModel model)
        {
            var list = _service.Query;
            if (model.LogId != 0)
            {
                list = list.Where(t=>t.LogId == model.LogId);
            }
            if (model.LogType != null && model.LogType!=0)
            {
                list = list.Where(t=>t.LogType==model.LogType);
            }
            if (!string.IsNullOrWhiteSpace(model.Mid))
            {
                list = list.Where(t=>t.Mid == model.Mid);
            }
            if (!string.IsNullOrWhiteSpace(model.Mname))
            {
                list = list.Where(t=>t.Mname == model.Mname);
            }
            list = list.ListPage(model.PageIndex, model.PageSize, out int totalCount);
            return new PageData<List<SysOplogDTO>>
            {
                Total = totalCount,
                Data = list.ToList()
            };
        }
    }
}