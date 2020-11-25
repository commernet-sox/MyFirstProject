using CPC;
using System;
using System.Collections.Generic;

namespace Data.IdentityService
{
    public partial class AuthAppParameterDTO : IMapDto
    {
        public long FlowNo { get; set; }
        public string AppKey { get; set; }
        public string PartnerType { get; set; }
        public string PartnerCode { get; set; }
        public string PartnerName { get; set; }
        public string ParmKey { get; set; }
        public string ParmName { get; set; }
        public string ParmValue { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string Memo { get; set; }
    }
}
