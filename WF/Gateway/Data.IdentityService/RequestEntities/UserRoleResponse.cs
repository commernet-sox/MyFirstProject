using CPC;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.IdentityService
{
    [MapperProperty(MapName = "SysMenu")]
    public class UserRoleResponse : SysMenuDTO
    {
        public SysCommandDTO[] Commands { get; set; }
    }
}
