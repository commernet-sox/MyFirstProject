using CPC;
using System;
using System.Collections.Generic;

namespace Infrastructure.IdentityService.Models
{
    public partial class SysSystem : IMapEntity
    {
        public string SysCode { get; set; }
        public string SysName { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? Status { get; set; }
        public string Description { get; set; }
        public string Memo { get; set; }
    }
}
