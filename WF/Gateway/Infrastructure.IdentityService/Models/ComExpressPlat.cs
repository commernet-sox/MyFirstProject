using CPC;
using System;
using System.Collections.Generic;

namespace Infrastructure.IdentityService.Models
{
    public partial class ComExpressPlat : IMapEntity
    {
        public string PlatCode { get; set; }
        public string PlatCarrierCode { get; set; }
        public string OwnCarrierCode { get; set; }
        public string Remark { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
