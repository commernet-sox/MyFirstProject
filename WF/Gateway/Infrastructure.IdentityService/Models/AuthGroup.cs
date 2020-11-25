using CPC;
using System;
using System.Collections.Generic;

namespace Infrastructure.IdentityService.Models
{
    public partial class AuthGroup : IMapEntity
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
    }
}
