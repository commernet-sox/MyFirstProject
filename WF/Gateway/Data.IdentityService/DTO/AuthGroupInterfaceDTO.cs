using CPC;
using System;
using System.Collections.Generic;

namespace Data.IdentityService
{
    public partial class AuthGroupInterfaceDTO : IMapDto
    {
        public string GroupId { get; set; }
        public string InterfaceId { get; set; }
    }
}
