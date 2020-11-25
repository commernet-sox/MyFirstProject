using CPC;
using System;
using System.Collections.Generic;

namespace Data.IdentityService
{
    public partial class AuthInterfaceDTO : IMapDto
    {
        public string InterfaceId { get; set; }
        public string InterfaceName { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public string Memo { get; set; }
        public string GrantType { get; set; }
    }
}
