using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.Controllers
{
    public class FirstApiController:ApiController
    {
        [HttpGet]
        public string Get()
        {
            return @"我是FirstApiController！";
        }
    }
}
