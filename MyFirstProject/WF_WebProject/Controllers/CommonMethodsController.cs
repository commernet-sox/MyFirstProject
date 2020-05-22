using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.WebServices.Model;
using Microsoft.AspNetCore.Mvc;
using WFWebProject.Interface;

namespace WFWebProject.Controllers
{
    public class CommonMethodsController : Controller
    {
        private IUserService _userService;
        private IMenuInfoService _menuInfoService;
        public CommonMethodsController(IUserService userService,IMenuInfoService menuInfoService)
        {
            _userService = userService;
            _menuInfoService = menuInfoService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetUserInfo(string term, string value, int page = 0)
        {
            
            var query = this._userService.GetAll();
            if (!string.IsNullOrEmpty(term))
            {
                query = query.Where(p => p.UserName.Contains(term) || p.Tel.Contains(term));  //同时使用用户名和手机号码查询
            }
            if (!string.IsNullOrEmpty(value))
            {
                int m_value = 0;
                if (int.TryParse(value, out m_value))
                {
                    query = query.Where(p => p.Id == m_value);
                }
            }
            Select2Model sm = new Select2Model();
            sm.incomplete_results = true;
            sm.total_count = query.Count();
            
             var data = query.OrderBy(o => o.UserName).Skip(page * 30).Take(30).Select(s => new { id = s.Id, text = s.UserName });
                sm.items = data.ToList();
            
            return Json(sm);
        }
        
        
        public ActionResult UserPermissionTreeData(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var all_warehouses = this._menuInfoService.GetAll().GroupBy(t => t.Title).ToList();
                List<JSTreeModel> jstree = new List<JSTreeModel>();
                if (id == "#")
                {
                    foreach (var p in all_warehouses)
                    {
                        var mx = p.FirstOrDefault();
                        jstree.Add(new Core.WebServices.Model.JSTreeModel()
                        {

                            id = "M#" + mx.TitleId,
                            parent = "#",
                            text = mx.Title,
                            icon = "fa fa-globe icon-lg",
                            children = false,
                            state = new Core.WebServices.Model.JSTreeState { selected = false }
                        });
                        foreach (var q in p)
                        {
                            jstree.Add(new Core.WebServices.Model.JSTreeModel()
                            {
                                id = "D#" + q.Id.ToString(),
                                parent = "M#" + q.TitleId,
                                text = q.Content,
                                icon = "fa fa-globe icon-lg",
                                children = false,
                                state = new Core.WebServices.Model.JSTreeState { selected = false }
                            });
                        }
                    }
                }
                return Json(jstree);
            }
            throw new ArgumentNullException();
        }
    }
}