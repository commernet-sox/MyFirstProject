using System;
using System.Collections.Generic;
using System.Linq;
using Data.IdentityService;
using Data.IdentityService.Model;
using CPC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;

namespace IdentityService.Controllers
{
    public class RoleController : BaseController
    {
        private readonly SysRoleService _service;
        public RoleController(SysRoleService service)
        {
            _service = service;
        }

        [HttpPost("GetList")]
        public PageData<List<SysRoleDTO>> GetList(RoleModel model)
        {
            var query = _service.Query;
            if (!string.IsNullOrEmpty(model.RoleId))
            {
                query = query.Where(t=>t.RoleId==model.RoleId);
            }
            if (!string.IsNullOrEmpty(model.RoleName))
            {
                query = query.Where(t => t.RoleName == model.RoleName);
            }
            query= query.Where(item => model.SysCode.Contains(item.SysCode))
                .ListPage(model.PageIndex, model.PageSize, out int totalCount);
            var list = query.ToList();
            return new PageData<List<SysRoleDTO>>
            {
                Total = totalCount,
                Data = list
            };
        }

        [HttpPost("Insert")]
        public Outcome Insert(SysRoleDTO dto)
        {
            dto.CreateBy = UserId;
            dto.CreateDate = DateTime.Now;
            Outcome outcome = new Outcome();
            var existItem = _service.Query.FirstOrDefault(item => item.RoleId == dto.RoleId);
            if (existItem != null)
            {
                outcome.Code = ApiCode.InvalidData;
                outcome.Message = string.Format("角色编码{0}已存在，请重新输入！", dto.RoleId);
                return outcome;
            }
            var flag = _service.Add(dto) != null;
            if (flag)
            {
                outcome.Code = ApiCode.Success;
                var menu = GetMenuInfo("role");
                if (menu != null)
                {
                    WriteLog(0, menu.MID, menu.MName, "insert", "新增", dto.RoleId, "sys_role");
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
        public Outcome Update(SysRoleDTO dto)
        {
            dto.ModifyBy = UserId;
            dto.ModifyDate = DateTime.Now;
            Outcome outcome = new Outcome();
            var flag = _service.Update(dto);
            if (flag)
            {
                outcome.Code = ApiCode.Success;
                var menu = GetMenuInfo("role");
                if (menu!=null)
                {
                    WriteLog(0, menu.MID, menu.MName, "modify", "新增", dto.RoleId, "sys_role");
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