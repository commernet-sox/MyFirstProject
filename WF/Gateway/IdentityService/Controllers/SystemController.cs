using System;
using System.Collections.Generic;
using System.Linq;
using Data.IdentityService;
using Data.IdentityService.Model;
using CPC;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    public class SystemController : BaseController
    {
        private readonly SysSystemService _service;
        public SystemController(SysSystemService service)
        {
            _service = service;
        }

        [HttpGet("GetList")]
        public PageData<List<SysSystemDTO>> GetList([FromQuery]SystemModel model)
        {
            var query = _service.Query;
            if (!string.IsNullOrEmpty(model.SysCode))
            {
                query = query.Where(t=>t.SysCode==model.SysCode);
            }
            if (!string.IsNullOrEmpty(model.SysName))
            {
                query = query.Where(t => t.SysName == model.SysName);
            }
            query = query.ListPage(model.PageIndex, model.PageSize, out int totalCount);
            var list = query.ToList();
            return new PageData<List<SysSystemDTO>>
            {
                Total = totalCount,
                Data = list
            };
        }

        [HttpPost("Insert")]
        public Outcome Insert(SysSystemDTO dto)
        {
            dto.CreateBy = UserId;
            dto.CreateDate = DateTime.Now;
            Outcome outcome = new Outcome();
            var existItem = _service.Query.FirstOrDefault(item => item.SysCode == dto.SysCode);
            if (existItem != null)
            {
                outcome.Code = ApiCode.InvalidData;
                outcome.Message = string.Format("系統编码{0}已存在，请重新输入！", dto.SysCode);
                return outcome;
            }
            var flag = _service.Add(dto) != null;
            if (flag)
            {
                outcome.Code = ApiCode.Success;
            }
            else
            {
                outcome.Code = ApiCode.InvalidData;
                outcome.Message = "插入失败，请检查数据！";
            }
            return outcome;
        }

        [HttpPost("Update")]
        public Outcome Update(SysSystemDTO dto)
        {
            dto.ModifyBy = UserId;
            dto.ModifyDate = DateTime.Now;
            Outcome outcome = new Outcome();
            var flag = _service.Update(dto);
            if (flag)
            {
                outcome.Code = ApiCode.Success;
            }
            else
            {
                outcome.Code = ApiCode.InvalidData;
                outcome.Message = "更新失败，请检查数据！";
                return outcome;
            }
            return outcome;
        }
    }
}