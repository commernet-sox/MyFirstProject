using CPC;
using System;
using System.Collections.Generic;

namespace Infrastructure.IdentityService.Models
{
    public partial class AuthAppGroup : IMapEntity
    {
        public string AppKey { get; set; }
        public string GroupId { get; set; }
    }
}
