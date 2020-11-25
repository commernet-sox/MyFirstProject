using CPC;
using System;
using System.Collections.Generic;

namespace Infrastructure.IdentityService.Models
{
    public partial class SysCommand : IMapEntity
    {
        public string Mid { get; set; }
        public string CommandCode { get; set; }
        public string CommandName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string Location { get; set; }
        public string Icon { get; set; }
    }
}
