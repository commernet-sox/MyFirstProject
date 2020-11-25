using CPC;
using System;
using System.Collections.Generic;

namespace Infrastructure.IdentityService.Models
{
    public partial class AuthApp : IMapEntity
    {
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public string SessionKey { get; set; }
        public string RedirectUrl { get; set; }
        public string AppName { get; set; }
        public string AppIcon { get; set; }
        public string AppType { get; set; }
        public string AppClass { get; set; }
        public bool? Status { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string Memo { get; set; }
    }
}
