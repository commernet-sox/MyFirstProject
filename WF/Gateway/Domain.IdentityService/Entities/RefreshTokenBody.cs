using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.IdentityService
{
    public class RefreshTokenBody
    {
        public string ClientId { get; set; }

        public string UserName { get; set; }

        public string Scope{ get; set; }
    }
}
