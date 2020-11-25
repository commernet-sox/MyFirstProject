using System;
using System.Collections.Generic;
using System.Linq;
using Data.IdentityService;
using Data.IdentityService.Model;
using CPC;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    public class DepartmentController : BaseController
    {
        private readonly SysDepartmentService _service;

        public DepartmentController(SysDepartmentService service)
        {
            _service = service;
        }

        [HttpGet("GetList")]
        public PageData<List<SysDepartmentDTO>> GetList([FromQuery] DepartmentModel model)
        {
            var list = _service.Query;
            if (!string.IsNullOrWhiteSpace(model.CompanyId))
            {
                list = list.Where(t => t.CompanyId == model.CompanyId);
            }
            if (!string.IsNullOrWhiteSpace(model.DepartmentId))
            {
                list = list.Where(t => t.DepartmentId == model.DepartmentId);
            }
            if (!string.IsNullOrWhiteSpace(model.DepartmentName))
            {
                list = list.Where(t => t.DepartmentName == model.DepartmentName);
            }

            list = list.ListPage(model.PageIndex, model.PageSize, out int totalCount);
            return new PageData<List<SysDepartmentDTO>>
            {
                Total = totalCount,
                Data = list.ToList()
            };
        }

        [HttpPost("Insert")]
        public Outcome Insert(SysDepartmentDTO dto)
        {
            dto.CreateBy = UserId;
            dto.CreateDate = DateTime.Now;
            Outcome outcome = new Outcome();
            var existItem = _service.Query.FirstOrDefault(item => item.DepartmentId == dto.DepartmentId);
            if (existItem != null)
            {
                outcome.Code = ApiCode.InvalidData;
                outcome.Message = string.Format("部门编码{0}已存在，请重新输入！", dto.DepartmentId);
                return outcome;
            }
            var flag = _service.Add(dto) != null;
            if (flag)
            {
                outcome.Code = ApiCode.Success;
                var menu = GetMenuInfo("department");
                WriteLog(0, menu.MID, menu.MName, "insert", "新增", dto.DepartmentId, "sys_department");
            }
            else
            {
                outcome.Code = ApiCode.InvalidData;
                outcome.Message = "插入失败，请检查数据！";
            }
            return outcome;
        }

        [HttpPost("Update")]
        public Outcome Update(SysDepartmentDTO dto)
        {
            dto.ModifyBy = UserId;
            dto.ModifyDate = DateTime.Now;
            Outcome outcome = new Outcome();
            var flag = _service.Update(dto);
            if (flag)
            {
                outcome.Code = ApiCode.Success;
                var menu = GetMenuInfo("department");
                WriteLog(0, menu.MID, menu.MName, "modify", "修改", dto.DepartmentId, "sys_department");
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