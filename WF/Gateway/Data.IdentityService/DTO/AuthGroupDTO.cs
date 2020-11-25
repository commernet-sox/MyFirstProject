using CPC;
using System;
using System.Collections.Generic;

namespace Data.IdentityService
{
    public partial class AuthGroupDTO : IMapDto
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
    }
}
