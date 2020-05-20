using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WFWebProject.Models;

namespace WFWebProject.ViewComponents
{
    public class Menu : ViewComponent
    {
            public Menu()
            {
            }

            public IViewComponentResult Invoke()
            {
                MenuModel menu = new MenuModel();
                List<string> list = new List<string>();
                list.Add("企业基本信息");
                list.Add("建造师信息");
                menu.MenuName = list;
                return View(menu);
            }
        
    }
}
