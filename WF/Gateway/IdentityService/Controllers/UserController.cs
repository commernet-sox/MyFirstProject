using Data.IdentityService;
using Data.IdentityService.DTO;
using Data.IdentityService.Model;
using IdentityService.Application.Service;
using Infrastructure.IdentityService.Models;
using AutoMapper;
using CPC;
using CPC.DBCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityService.Controllers
{
    public class UserController : BaseController
    {
        private readonly SysUserService _service;
        private readonly SysUserSystemService _sysUser;
        public UserController(SysUserService service, SysUserSystemService userSystemService)
        {
            _service = service;
            _sysUser = userSystemService;
        }

        [HttpGet("GetEmployee")]
        public ActionResult<SysEmployeeDTO> GetEmployee()
        {
            using (var db = GlobalContext.Resolve<AMSContext>())
            {
                var sql = string.Format("SELECT B.*,A.UserId,A.UserPwd from sys_user A LEFT JOIN sys_employee B on A.EmployeeId=B.EmployeeId where A.UserId='{0}'", UserId);
                SysEmployeeDTO sysEmployeeDTO = db.FromSql(sql).Query<SysEmployeeDTO>().FirstOrDefault();
                sysEmployeeDTO.UserPwd = CPC.DEncryptHelper.Decrypt(sysEmployeeDTO.UserPwd);
                return sysEmployeeDTO;
            }
        }

        /// <summary>
        /// 获取当前登录用户的功能权限
        /// </summary>
        /// <param name="sysCode">系统编码</param>
        /// <returns></returns>
        [HttpGet("Role")]
        public ActionResult<List<UserRoleResponse>> GetRole(string sysCode)
        {
            var mapper = GlobalContext.Resolve<IMapper>();

            using var db = GlobalContext.Resolve<AMSContext>();

            var list = (from r in db.SysRole
                        where r.SysCode == sysCode
                        join ru in db.SysRoleuser on r.RoleId equals ru.RoleId
                        where ru.UserId == UserId
                        join rm in db.SysRolemenu on r.RoleId equals rm.RoleId
                        join c in db.SysCommand on new { rm.Mid, rm.CommandId } equals new { c.Mid, CommandId = c.CommandCode }
                        select c).Map<SysCommandDTO>().Distinct().ToList();

            if (list.IsNull())
            {
                return NoContent();
            }

            var menuList = db.SysMenu.Where(t => list.Select(t => t.Mid).Distinct().Contains(t.Mid)).Map<UserRoleResponse>().ToList();

            if (menuList.IsNull())
            {
                return NoContent();
            }

            menuList.ForEach(t => t.Commands = list.Where(c => c.Mid == t.Mid).ToArray());

            return menuList;
        }


        [HttpGet("GetList")]
        public PageData<List<SysUserDTO>> GetList([FromQuery] UserModel model)
        {
            var query = _service.Query;
            if (!string.IsNullOrEmpty(model.EmployeeId))
            {
                query = query.Where(t => t.EmployeeId == model.EmployeeId);
            }
            if (!string.IsNullOrEmpty(model.PersonName))
            {
                query = query.Where(t=>t.PersonName==model.PersonName);
            }
            query = query.ListPage(model.PageIndex, model.PageSize, out int totalCount);
            var list = query.ToList();
            list.ForEach(t => t.UserPwd = CPC.DEncryptHelper.Decrypt(t.UserPwd));
            list.ForEach(t => t.Pdapwd = CPC.DEncryptHelper.Decrypt(t.Pdapwd));
            foreach (var item in list)
            {
                List<SysUserSystemDTO> list1= _sysUser.Query.Where(t => t.UserId == item.UserId).ToList();
                List<string> sysCodeList = new List<string>();
                foreach (var it in list1)
                {
                    sysCodeList.Add(it.SysCode);
                }
                item.SysCode = sysCodeList.ToArray();
            }
            return new PageData<List<SysUserDTO>>
            {
                Total = totalCount,
                Data = list
            };
        }

        [HttpPost("Insert")]
        public Outcome Insert(SysUserDTO dto)
        {
            dto.CreateBy = UserId;
            dto.CreateDate = DateTime.Now;
            dto.UserPwd = CPC.DEncryptHelper.Encrypt(dto.UserPwd);
            dto.DefaultCkId = "1";
            if (!string.IsNullOrEmpty(dto.Pdapwd))
            {
                dto.Pdapwd = CPC.DEncryptHelper.Encrypt(dto.Pdapwd);
            }
            Outcome outcome = new Outcome();
            var existItem = _service.Query.FirstOrDefault(item => item.UserId == dto.UserId);
            if (existItem != null)
            {
                outcome.Code = ApiCode.InvalidData;
                outcome.Message = string.Format("用户编码{0}已存在，请重新输入！", dto.UserId);
                return outcome;
            }
            var flag = _service.Add(dto) != null;
            using (var db = GlobalContext.Resolve<AMSContext>())
            {
                foreach (var item in dto.SysCode)
                {
                    SysUserSystem sysUser = new SysUserSystem();
                    sysUser.UserId = dto.UserId;
                    sysUser.SysCode = item;
                    db.SysUserSystems.Add(sysUser);
                }
                db.SaveChanges();
            }
            if (flag)
            {
                outcome.Code = ApiCode.Success;
                var menu = GetMenuInfo("user");
                WriteLog(0, menu.MID, menu.MName, "insert", "新增", dto.UserId, "sys_user");
            }
            else
            {
                outcome.Code = ApiCode.InvalidData;
                outcome.Message = "插入失败，请检查数据！";
            }
            return outcome;
        }

        [HttpPost("Update")]
        public Outcome Update(SysUserDTO dto)
        {
            dto.ModifyBy = UserId;
            dto.ModifyDate = DateTime.Now;
            dto.UserPwd = CPC.DEncryptHelper.Encrypt(dto.UserPwd);
            if (!string.IsNullOrEmpty(dto.Pdapwd))
            {
                dto.Pdapwd = CPC.DEncryptHelper.Encrypt(dto.Pdapwd);
            }
            Outcome outcome = new Outcome();
            var flag = _service.Update(dto);
            using (var db = GlobalContext.Resolve<AMSContext>())
            {
                var userSystems = db.SysUserSystems.Where(i => i.UserId == dto.UserId);
                if (userSystems != null && userSystems.Count() > 0)
                {
                    db.SysUserSystems.RemoveRange(userSystems);
                }
                foreach (var item in dto.SysCode)
                {
                    SysUserSystem sysUser = new SysUserSystem();
                    sysUser.UserId = dto.UserId;
                    sysUser.SysCode = item;
                    db.SysUserSystems.Add(sysUser);
                }
                db.SaveChanges();
            }
            if (flag)
            {
                outcome.Code = ApiCode.Success;
                var menu = GetMenuInfo("user");
                WriteLog(0, menu.MID, menu.MName, "modify", "修改", dto.UserId, "sys_user");
            }
            else
            {
                outcome.Code = ApiCode.InvalidData;
                outcome.Message = "更新失败，请检查数据！";
                return outcome;
            }
            return outcome;
        }

        [HttpPost("PwdUpdate")]
        public Outcome PwdUpdate(SysSecuritySettingDTO dto)
        {
            Outcome outcome = new Outcome();
            if (dto.NewPwd != dto.ComfirmPwd)
            {
                outcome.Code = ApiCode.InvalidData;
                outcome.Message = "新密码输入不一致，请检查数据！";
                return outcome;
            }
            SysUserDTO obj;
            using (var db = GlobalContext.Resolve<AMSContext>())
            {
                var sql = string.Format("select * from sys_user where userid='{0}'", UserId);
                //var user=db.SysUser.Where(t => t.UserId == UserId).FirstOrDefault();
                //var obj1= db.FromSql(sql).Query<SysUserDTO>().FirstOrDefault();
                var user = _service.Query.Where(t => t.UserId == UserId).FirstOrDefault();
                obj = user;
            }
            var typepwd = "";
            if (dto.PwdType == "0")
            {
                typepwd = CPC.DEncryptHelper.Decrypt(obj.UserPwd);
            }
            else
            {
                typepwd = CPC.DEncryptHelper.Decrypt(obj.Pdapwd);
            }
            if (dto.OldPwd != typepwd)
            {
                outcome.Code = ApiCode.InvalidData;
                outcome.Message = "原密码输入不正确，请检查数据！";
                return outcome;
            }
            else
            {
                if (dto.PwdType == "0")
                {
                    obj.UserPwd = dto.NewPwd;
                    obj.ModifyBy = UserId;
                    obj.ModifyDate = DateTime.Now;
                    obj.UserPwd = CPC.DEncryptHelper.Encrypt(obj.UserPwd);
                    if (!string.IsNullOrEmpty(obj.Pdapwd))
                    {
                        obj.Pdapwd = CPC.DEncryptHelper.Encrypt(obj.Pdapwd);
                    }
                    
                    var flag = _service.Update(obj);
                    if (flag)
                    {
                        outcome.Code = ApiCode.Success;
                        var menu = GetMenuInfo("user");
                        WriteLog(0, menu.MID, menu.MName, "modify", "修改", obj.UserId, "sys_user");
                    }
                    else
                    {
                        outcome.Code = ApiCode.InvalidData;
                        outcome.Message = "更新失败，请检查数据！";
                        return outcome;
                    }
                    return outcome;
                }
                else
                {
                    obj.Pdapwd = dto.NewPwd;
                    obj.ModifyBy = UserId;
                    obj.ModifyDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(obj.Pdapwd))
                    {
                        obj.Pdapwd = CPC.DEncryptHelper.Encrypt(obj.Pdapwd);
                    }
                    
                    var flag = _service.Update(obj);
                    if (flag)
                    {
                        outcome.Code = ApiCode.Success;
                        var menu = GetMenuInfo("user");
                        WriteLog(0, menu.MID, menu.MName, "modify", "修改", obj.UserId, "sys_user");
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
    }
}