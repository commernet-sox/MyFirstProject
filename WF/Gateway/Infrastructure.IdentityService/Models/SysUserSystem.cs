using CPC;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.IdentityService.Models
{
    public partial class SysUserSystem: IMapEntity
    {
        public string UserId { get; set; }
        public string SysCode { get; set; }
    }
}
