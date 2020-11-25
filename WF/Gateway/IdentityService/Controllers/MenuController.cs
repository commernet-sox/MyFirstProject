using System;
using System.Collections.Generic;
using System.Linq;
using Data.IdentityService;
using Data.IdentityService.Model;
using Infrastructure.IdentityService.Models;
using AutoMapper;
using CPC;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    public class MenuController : BaseController
    {
        private readonly SysMenuService _service;
        public MenuController(SysMenuService service)
        {
            _service = service;
        }

        [HttpGet("GetList")]
        public PageData<List<SysMenuDTO>> GetList([FromQuery]MenuModel model)
        {
            var list = _service.Query;
            if (!string.IsNullOrWhiteSpace(model.MID))
            {
                list = list.Where(t=>t.Mid == model.MID);
            }
            if (!string.IsNullOrWhiteSpace(model.MName))
            {
                list = list.Where(t=>t.Mname == model.MName);
            }
            list = list.Where(item => item.SysCode == model.SysCode)
                .ListPage(model.PageIndex, model.PageSize, out int totalCount)
                .OrderBy(item => item.Mid);
            return new PageData<List<SysMenuDTO>>
            {
                Total = totalCount,
                Data = list.ToList()
            };
        }

        [HttpGet("GetCommands")]
        public List<SysCommand> GetCommands(string mid)
        {
            using (var db = GlobalContext.Resolve<AMSContext>())
            {
                return db.SysCommand.Where(item => item.Mid == mid).ToList();
            }
        }

        [HttpPost("Insert")]
        public Outcome Insert(MenuCommandModel model)
        {
            Outcome outcome = new Outcome();
            model.Menu.CreateBy = UserId;
            model.Menu.CreateDate = DateTime.Now;
            using (var db = GlobalContext.Resolve<AMSContext>())
            {
                var existItem = db.SysMenu.FirstOrDefault(item => item.Mid == model.Menu.Mid);
                if (existItem != null)
                {
                    outcome.Code = ApiCode.InvalidData;
                    outcome.Message = string.Format("菜单编码{0}已存在，请重新输入！", model.Menu.Mid);
                    return outcome;
                }
                var mapper = GlobalContext.Resolve<IMapper>();
                var menu = mapper.Map<SysMenu>(model.Menu);
                db.SysMenu.Add(menu);

                if (model.Commands != null && model.Commands.Length > 0)
                {
                    foreach (var item in model.Commands)
                    {
                        item.CreateDate = DateTime.Now;
                        item.CreateBy = UserId;
                        var command = mapper.Map<SysCommand>(item);
                        db.SysCommand.Add(command);
                    }
                }
                db.SaveChanges();
            }
            outcome.Code = ApiCode.Success;
            var menuItem = GetMenuInfo("menu");
            WriteLog(0, menuItem.MID, menuItem.MName, "insert", "新增", model.Menu.Mid, "sys_menu");
            return outcome;
        }

        [HttpPost("Update")]
        public Outcome Update(MenuCommandModel model)
        {
            model.Menu.ModifyBy = UserId;
            model.Menu.ModifyDate = DateTime.Now;
            Outcome outcome = new Outcome();
            using (var db = GlobalContext.Resolve<AMSContext>())
            {
                var mapper = GlobalContext.Resolve<IMapper>();
                var menu = mapper.Map<SysMenu>(model.Menu);
                db.SysMenu.Update(menu);

                var commands = db.SysCommand.Where(item => item.Mid == model.Menu.Mid);
                if (commands != null && commands.Count() > 0)
                {
                    db.SysCommand.RemoveRange(commands);
                }
                if (model.Commands != null && model.Commands.Length > 0)
                {
                    foreach (var item in model.Commands)
                    {
                        item.CreateDate = DateTime.Now;
                        item.CreateBy = UserId;
                        var command = mapper.Map<SysCommand>(item);
                        db.SysCommand.Add(command);
                    }
                }
                db.SaveChanges();
            }
            outcome.Code = ApiCode.Success;
            var menuItem = GetMenuInfo("menu");
            if (menuItem!=null)
            {
                WriteLog(0, menuItem.MID, menuItem.MName, "modify", "修改", model.Menu.Mid, "sys_menu");
            }
            return outcome;
        }
    }
}