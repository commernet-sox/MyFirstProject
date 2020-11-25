using CPC;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.IdentityService.DTO
{
    public class SysUserSystemDTO: IMapDto
    {
        public string UserId { get; set; }
        public string SysCode { get; set; }
    }
}
