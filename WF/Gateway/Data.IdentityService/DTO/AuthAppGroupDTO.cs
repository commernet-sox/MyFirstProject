using CPC;
using System;
using System.Collections.Generic;

namespace Data.IdentityService
{
    public partial class AuthAppGroupDTO : IMapDto
    {
        public string AppKey { get; set; }
        public string GroupId { get; set; }
    }
}
