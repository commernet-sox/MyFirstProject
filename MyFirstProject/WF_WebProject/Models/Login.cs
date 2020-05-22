using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WFWebProject.Models
{
    public class Login
    {
        [Required(ErrorMessage = "用户名不能为空。")]
        public string Tel { get; set; }
        [Required(ErrorMessage = "密码不能为空。")]
        [DataType(DataType.Password)]
        public string Pwd { get; set; }
        public bool RememberMe { get; set; }
       
    }
}
