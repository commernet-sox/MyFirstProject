using System;
using System.Collections.Generic;
using System.Linq;
using Data.IdentityService;
using Data.IdentityService.Model;
using CPC;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly SysCompanyService _service;

        public CompanyController(SysCompanyService service)
        {
            _service = service;
        }

        [HttpGet("GetList")]
        public PageData<List<SysCompanyDTO>> GetList([FromQuery] CompanyModel model)
        {
            var list = _service.Query;

            if (!string.IsNullOrWhiteSpace(model.CompanyId))
            {
                list = list.Where(t => t.CompanyId == model.CompanyId);
            }
            if (!string.IsNullOrWhiteSpace(model.CompanyName))
            {
                list = list.Where(t => t.CompanyName == model.CompanyName);
            }
            list = list.ListPage(model.PageIndex, model.PageSize, out int totalCount);
            return new PageData<List<SysCompanyDTO>>
            {
                Total = totalCount,
                Data = list.ToList()
            };
        }

        [HttpPost("Insert")]
        public Outcome Insert(SysCompanyDTO dto)
        {
            dto.CreateBy = UserId;
            dto.CreateDate = DateTime.Now;
            Outcome outcome = new Outcome();
            var existItem = _service.Query.FirstOrDefault(item => item.CompanyId == dto.CompanyId);
            if (existItem != null)
            {
                outcome.Code = ApiCode.InvalidData;
                outcome.Message = string.Format("公司编码{0}已存在，请重新输入！", dto.CompanyId);
                return outcome;
            }
            var flag = _service.Add(dto) != null;
            if (flag)
            {
                outcome.Code = ApiCode.Success;
                var menu = GetMenuInfo("company");
                if (menu != null)
                {
                    WriteLog(0, menu.MID, menu.MName, "insert", "新增", dto.CompanyId, "sys_company");
                }
            }
            else
            {
                outcome.Code = ApiCode.InvalidData;
                outcome.Message = "插入失败，请检查数据！";
            }
            return outcome;
        }

        [HttpPost("Update")]
        public Outcome Update(SysCompanyDTO dto)
        {
            dto.ModifyBy = UserId;
            dto.ModifyDate = DateTime.Now;
            Outcome outcome = new Outcome();
            var flag = _service.Update(dto);
            if (flag)
            {
                outcome.Code = ApiCode.Success;
                var menu = GetMenuInfo("company");
                if (menu != null)
                {
                    WriteLog(0, menu.MID, menu.MName, "modify", "修改", dto.CompanyId, "sys_company");
                }
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