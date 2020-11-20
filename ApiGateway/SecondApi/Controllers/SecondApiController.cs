using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondApi.Controllers
{
    public class SecondApiController:ApiController
    {
        [HttpGet]
        public string Get()
        {
            return @"SecondApiController！";
        }
    }
}
