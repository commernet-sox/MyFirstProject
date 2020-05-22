using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WFWebProject.Models;

namespace WFWebProject.ViewComponents
{
    public class Menu : ViewComponent
    {
        private DataContext DataContext;
        public Menu(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public IViewComponentResult Invoke()
        {
            DataContext dataContext;
            var Tel= HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(Tel))
            {
                var UserId = DataContext.User.Where(t => t.UserName == Tel).FirstOrDefault();
                var content = DataContext.UserMenuRole.Where(t => t.UserId == UserId.Id.ToString()).Select(t => t.Content).ToList();
                MenuModel menu = new MenuModel();
                List<string> list = new List<string>();
                foreach (var item in content)
                {
                    var text = DataContext.MenuInfo.Where(t => t.Id.ToString() == item).FirstOrDefault();
                    list.Add(text.Title);
                    list.Add(text.Content);
                }
                menu.MenuName = list;
                return View(menu);
            }
            else
            {
                return View();
            }
            
        }
        
    }
}
