using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SimpleWebApi.Data.Requests
{
    public class TestApiAddRequest
    {
        [Required(ErrorMessage ="姓名不能为空")]
        public string Name { get; set; }
        [Required(ErrorMessage = "年龄不能为空")]
        public int Age { get; set; }
    }
}
