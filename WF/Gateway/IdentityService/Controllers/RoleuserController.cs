using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.IdentityService;
using Data.IdentityService.Model;
using IdentityService.Application.Service;
using Infrastructure.IdentityService.Models;
using AutoMapper;
using CPC;
using CPC.DBCore;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    public class RoleuserController : BaseController
    {
        private readonly SysRoleuserService _service;
        public RoleuserController(SysRoleuserService service)
        {
            _service = service;
        }
        [HttpGet("GetList")]
        public PageData<List<SysRoleuserDTO>> GetList([FromQuery] RoleuserModel model)
        {
            var list = _service.Query.ListPage(model.PageIndex, model.PageSize, out int totalCount).ToList();
            return new PageData<List<SysRoleuserDTO>>
            {
                Total = totalCount,
                Data = list
            };
        }
        [HttpPost("Insert")]
        public Outcome Insert(SysRoleuserDTO dto)
        {
            dto.CreateBy = UserId;
            dto.CreateDate = DateTime.Now;
            Outcome outcome = new Outcome();
            var existItem = _service.Query.FirstOrDefault(item => item.RoleId == dto.RoleId && item.UserId==dto.UserId);
            if (existItem != null)
            {
                outcome.Code = ApiCode.InvalidData;
                outcome.Message = string.Format("用户角色{0}已存在，请重新输入！", dto.RoleId);
                return outcome;
            }
            var flag = _service.Add(dto) != null;
            if (flag)
            {
                outcome.Code = ApiCode.Success;
                var menu = GetMenuInfo("roleuser");
                WriteLog(0, menu.MID, menu.MName, "insert", "新增", dto.RoleId+dto.UserId, "sys_roleuser");
            }
            else
            {
                outcome.Code = ApiCode.InvalidData;
                outcome.Message = "插入失败，请检查数据！";
            }
            return outcome;
        }

        [HttpPost("Update")]
        public Outcome Update(SysRoleuserDTO[] dto)
        {
            Outcome outcome = new Outcome();
            using (var db = GlobalContext.Resolve<AMSContext>())
            {
                var mapper = GlobalContext.Resolve<IMapper>();
                if (dto != null && dto.Length > 0)
                {
                    List<string> user = new List<string>();
                    foreach (var item in dto)
                    {
                        var roleMenus = db.SysRoleuser.Where(i => i.UserId == item.UserId);
                        if (roleMenus != null && roleMenus.Count() > 0)
                        {
                            db.SysRoleuser.RemoveRange(roleMenus);
                        }
                        if (!user.Contains(item.UserId))
                            user.Add(item.UserId);
                    }
                    foreach (var item1 in dto)
                    {
                        item1.Status = true;
                        item1.CreateDate = DateTime.Now;
                        item1.CreateBy = UserId;
                        //item1.UserId = item;
                        var roleMenu = mapper.Map<SysRoleuser>(item1);
                        db.SysRoleuser.Add(roleMenu);
                    }
                    db.SaveChanges();
                    var menu = GetMenuInfo("roleuser");
                    if (menu != null)
                    {
                        foreach (var item in user)
                        {
                            WriteLog(0, menu.MID, menu.MName, "modify", "修改", item, "sys_roleuser");
                        }
                    }
                }
            }
            outcome.Code = ApiCode.Success;
            return outcome;



            //Outcome outcome = new Outcome();
            //dto.Status = true;
            //dto.CreateBy = UserId;
            //dto.CreateDate = DateTime.Now;
            //var flag = _service.Update(dto);
            //if (flag)
            //{
            //    outcome.Code = ApiCode.Success;
            //    var menu = GetMenuInfo("roleuser");
            //    WriteLog(0, menu.MID, menu.MName, "modify", "修改",dto.RoleId + dto.UserId, "sys_roleuser");
            //}
            //else
            //{
            //    outcome.Code = ApiCode.InvalidData;
            //    outcome.Message = "更新失败，请检查数据！";
            //    return outcome;
            //}
            //return outcome;
        }

        [HttpGet("GetRoleUserMenu")]
        public List<SysRoleuser> GetRoleUserMenu(string UserId)
        {
            using (var db = GlobalContext.Resolve<AMSContext>())
            {
                return db.SysRoleuser.Where(item => item.UserId == UserId).ToList();
            }
        }

        [HttpGet("GetRoleUserCommand")]
        public List<MenuItemModel> GetRoleUserCommand()
        {
            using (var db = GlobalContext.Resolve<AMSContext>())
            {
                string sql = "SELECT CompanyId as Mid,CompanyName as Mname,'/' as Pid from sys_company";
                var menuItems = db.FromSql(sql).Query<MenuItemModel>().ToList();
                List<MenuItemModel> items = new List<MenuItemModel>();
                foreach (var item in menuItems)
                {
                    MenuItemModel menu1 = new MenuItemModel();
                    menu1.Type = "menu";
                    menu1.Mid = item.Mid;
                    menu1.Mname = item.Mname;
                    menu1.Pid = item.Pid;
                    menu1.IsLastLevel = 0;
                    sql = string.Format(@"SELECT DepartmentId as Mid,DepartmentName as Mname,CompanyId as Pid from sys_department WHERE CompanyId='{0}'", item.Mid);
                    var menuItems1 = db.FromSql(sql).Query<MenuItemModel>().ToList();
                    if (menuItems1.Count > 0)
                    {
                        List<MenuItemModel> list2 = new List<MenuItemModel>();
                        foreach (var item1 in menuItems1)
                        {
                            MenuItemModel menu2 = new MenuItemModel();
                            menu2.Type = "menu";
                            menu2.Mid = item1.Mid;
                            menu2.Mname = item1.Mname;
                            menu2.Pid = item1.Pid;
                            menu2.IsLastLevel = 0;
                            sql = string.Format("SELECT b.UserId as Mid,a.EmployeeName as Mname,a.DepartmentId as Pid from sys_employee a JOIN sys_user b ON a.EmployeeId=b.EmployeeId where a.DepartmentId='{0}'", item1.Mid);
                            var menuItems2 = db.FromSql(sql).Query<MenuItemModel>().ToList();
                            if (menuItems2.Count > 0)
                            {
                                List<MenuItemModel> list1 = new List<MenuItemModel>();
                                foreach (var item2 in menuItems2)
                                {
                                    MenuItemModel menu3 = new MenuItemModel();
                                    menu3.Type = "commmand";
                                    menu3.Mid = item2.Mid;
                                    menu3.Mname = item2.Mname;
                                    menu3.Pid = item2.Pid;
                                    menu3.IsLastLevel = 1;
                                    list1.Add(menu3);
                                }
                                menu2.items=list1.ToArray();
                            }
                            else
                            {
                                menu2.IsLastLevel = 1;
                            }
                            list2.Add(menu2);
                        }
                        menu1.items=list2.ToArray();
                    }
                    else
                    {
                        menu1.IsLastLevel = 1;
                    }
                    items.Add(menu1);
                }
                return items;
            }
        }
    }
}
