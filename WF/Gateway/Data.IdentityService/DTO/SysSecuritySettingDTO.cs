using CPC;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.IdentityService.DTO
{
    public class SysSecuritySettingDTO: IMapDto
    {
        public string PwdType { get; set; }
        public string OldPwd { get; set; }
        public string NewPwd { get; set; }
        public string ComfirmPwd { get; set; }
    }
}
