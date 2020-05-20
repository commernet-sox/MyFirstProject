using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WFWebProject.Models
{
    public class User
    {
        public int Id { get; set; }
        //用户名
        public string UserName { get; set; }
        //手机号
        public string Tel { get; set; }
        //密码
        public string Pwd { get; set; }
    }
}
