using System;
using System.Collections.Generic;
using System.Linq;
using Data.IdentityService;
using Data.IdentityService.Model;
using CPC;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    public class EmployeeController : BaseController
    {
        private readonly SysEmployeeService _service;

        public EmployeeController(SysEmployeeService service)
        {
            _service = service;
        }

        [HttpGet("GetList")]
        public PageData<List<SysEmployeeDTO>> GetList([FromQuery]EmployeeModel model)
        {
            var list = _service.Query;
            if (!string.IsNullOrWhiteSpace(model.CompanyId))
            {
                list = list.Where(t=>t.CompanyId == model.CompanyId);
            }
            if (!string.IsNullOrWhiteSpace(model.DepartmentId))
            {
                list = list.Where(t=>t.DepartmentId == model.DepartmentId);
            }
            if (!string.IsNullOrWhiteSpace(model.EmployeeId))
            {
                list = list.Where(t=>t.EmployeeId==model.EmployeeId);
            }
            if (!string.IsNullOrWhiteSpace(model.EmployeeName))
            {
                list = list.Where(t=>t.EmployeeName==model.EmployeeName);
            }
            list = list.ListPage(model.PageIndex, model.PageSize, out int totalCount);
            return new PageData<List<SysEmployeeDTO>>
            {
                Total = totalCount,
                Data = list.ToList()
            };
        }

        [HttpPost("Insert")]
        public Outcome Insert(SysEmployeeDTO dto)
        {
            dto.CreateBy = UserId;
            dto.CreateDate = DateTime.Now;
            Outcome outcome = new Outcome();
            var existItem = _service.Query.FirstOrDefault(item => item.EmployeeId == dto.EmployeeId);
            if (existItem != null)
            {
                outcome.Code = ApiCode.InvalidData;
                outcome.Message = string.Format("员工编码{0}已存在，请重新输入！", dto.EmployeeId);
                return outcome;
            }
            var flag = _service.Add(dto) != null;
            if (flag)
            {
                outcome.Code = ApiCode.Success;
                var menu = GetMenuInfo("employee");
                WriteLog(0, menu.MID, menu.MName, "insert", "新增", dto.EmployeeId, "sys_employee");
            }
            else
            {
                outcome.Code = ApiCode.InvalidData;
                outcome.Message = "插入失败，请检查数据！";
            }
            return outcome;
        }

        [HttpPost("Update")]
        public Outcome Update(SysEmployeeDTO dto)
        {
            dto.ModifyBy = UserId;
            dto.ModifyDate = DateTime.Now;
            Outcome outcome = new Outcome();
            var flag = _service.Update(dto);
            if (flag)
            {
                outcome.Code = ApiCode.Success;
                var menu = GetMenuInfo("employee");
                WriteLog(0, menu.MID, menu.MName, "modify", "修改", dto.EmployeeId, "sys_employee");
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