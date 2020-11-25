using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Data.IdentityService;
using Data.IdentityService.Model;
using Infrastructure.IdentityService;
using Infrastructure.IdentityService.Models;
using AutoMapper;
using CPC;
using CPC.DBCore;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    public class RoleMenuController : BaseController
    {
        public RoleMenuController()
        {
        }

        [HttpGet("GetMenuCommand")]
        public List<MenuItemModel> GetMenuCommand(string sysCode)
        {
            using (var db = GlobalContext.Resolve<AMSContext>())
            {
                string sql = string.Format(@"select a.Mid,a.Mname,a.Pid,a.IsLastLevel,b.CommandCode,b.CommandName 
from sys_menu a left join sys_command b on a.mid=b.mid 
where a.sysCode='{0}'", sysCode);
                var menuItems = db.FromSql(sql).Query<MenuItemModel>().ToList();
                List<MenuItemModel> items = new List<MenuItemModel>();
                RecursiveLoadData("/", menuItems, ref items);
                return items;
            }
        }

        private static void RecursiveLoadData(string pid, List<MenuItemModel> menuItems, ref List<MenuItemModel> items)
        {
            var rows = menuItems.Where(item => item.Pid == pid).ToList();
            if (rows != null && rows.Count > 0)
            {
                var lastRows = rows.Where(item => item.IsLastLevel == 1).ToList();
                var unLastRows = rows.Where(item => item.IsLastLevel == 0).ToList();
                if (lastRows.Count > 0)
                {
                    var groups = lastRows.GroupBy(item => item.Mid);
                    foreach (var group in groups)
                    {
                        var groupRow = group.FirstOrDefault();
                        var lastItem = new MenuItemModel
                        {
                            Type = "menu",
                            Mid = groupRow.Mid,
                            Mname = groupRow.Mname,
                            Pid = groupRow.Pid,
                            IsLastLevel = groupRow.IsLastLevel
                        };
                        items.Add(lastItem);

                        var lastItems = new List<MenuItemModel>();
                        foreach (var item in group.ToList())
                        {
                            if (string.IsNullOrWhiteSpace(item.CommandCode)) continue;
                            lastItems.Add(new MenuItemModel
                            {
                                Type = "commmand",
                                Mid = item.CommandCode,
                                Mname = item.CommandName,
                                Pid = item.Mid,
                                IsLastLevel = item.IsLastLevel
                            });
                        }
                        lastItem.items = lastItems.ToArray();
                    }
                }
                if (unLastRows.Count > 0)
                {
                    foreach (var row in unLastRows)
                    {
                        row.Type = "menu";
                        items.Add(row);
                        List<MenuItemModel> childItems = new List<MenuItemModel>();
                        RecursiveLoadData(row.Mid, menuItems, ref childItems);
                        if (childItems != null)
                        {
                            row.items = childItems.ToArray();
                        }
                    }
                }
            }
        }

        [HttpGet("GetRoleMenu")]
        public List<SysRolemenu> GetRoleMenu(string roleId)
        {
            using (var db = GlobalContext.Resolve<AMSContext>())
            {
                return db.SysRolemenu.Where(item => item.RoleId == roleId).ToList();
            }
        }

        [HttpPost("Insert")]
        public Outcome Insert(SysRolemenuDTO[] menus)
        {
            Outcome outcome = new Outcome();
            using (var db = GlobalContext.Resolve<AMSContext>())
            {
                if (menus != null && menus.Length > 0)
                {
                    var mapper = GlobalContext.Resolve<IMapper>();
                    foreach (var item in menus)
                    {
                        item.CreateDate = DateTime.Now;
                        item.CreateBy = UserId;
                        var roleMenu = mapper.Map<SysRolemenu>(item);
                        db.SysRolemenu.Add(roleMenu);
                    }
                }
                db.SaveChanges();
            }
            outcome.Code = ApiCode.Success;
            return outcome;
        }

        [HttpPost("Update")]
        public Outcome Update(SysRolemenuDTO[] menus)
        {
            Outcome outcome = new Outcome();
            using (var db = GlobalContext.Resolve<AMSContext>())
            {
                var mapper = GlobalContext.Resolve<IMapper>();
                if (menus != null && menus.Length > 0)
                {
                    var roleMenus = db.SysRolemenu.Where(item => item.RoleId == menus[0].RoleId);
                    if (roleMenus != null && roleMenus.Count() > 0)
                    {
                        db.SysRolemenu.RemoveRange(roleMenus);
                    }
                    foreach (var item in menus)
                    {
                        item.CommandId = item.CommandId == null ? string.Empty : item.CommandId;
                        item.CreateDate = DateTime.Now;
                        item.CreateBy = UserId;
                        var roleMenu = mapper.Map<SysRolemenu>(item);
                        db.SysRolemenu.Add(roleMenu);
                    }
                    db.SaveChanges();
                    var menu = GetMenuInfo("rolemenu");
                    if (menu!=null)
                    {
                        WriteLog(0, menu.MID, menu.MName, "modify", "修改", menus[0].RoleId, "sys_rolemenu");
                    }
                }
            }
            outcome.Code = ApiCode.Success;
            return outcome;
        }
    }
}