using CPC;
using System;
using System.Collections.Generic;

namespace Infrastructure.IdentityService.Models
{
    public partial class AuthGroupInterface : IMapEntity
    {
        public string GroupId { get; set; }
        public string InterfaceId { get; set; }
    }
}
