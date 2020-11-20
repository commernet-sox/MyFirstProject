﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewGateway.Controllers
{
    public class GatewayApiController:ApiController
    {
        [HttpGet]
        public string Get()
        {
            return @"我是api网关！我是微服务架构中的唯一入口，
                    我提供一个单独且统一的API入口用于访问内部一个或多个API。";
        }
    }
}
