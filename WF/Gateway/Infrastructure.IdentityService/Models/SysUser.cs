using CPC;
using System;
using System.Collections.Generic;

namespace Infrastructure.IdentityService.Models
{
    public partial class SysUser : IMapEntity
    {
        public string UserId { get; set; }
        public string UserPwd { get; set; }
        public string EmployeeId { get; set; }
        public string CompanyId { get; set; }
        public string DefaultCkId { get; set; }
        public bool Status { get; set; }
        public string Remark { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? ForbidLoginDate { get; set; }
        public int? EnforcePwdpolicy { get; set; }
        public int? EnforceExpirePolicy { get; set; }
        public string PwdpolicyType { get; set; }
        public string Ex1 { get; set; }
        public string Ex2 { get; set; }
        public int? Ex3 { get; set; }
        public int? Ex4 { get; set; }
        public string Pdapwd { get; set; }
        public string Sex { get; set; }
        public string PersonName { get; set; }
        public string IdNumber { get; set; }
        public string CellNumber { get; set; }
        public string UserType { get; set; }
        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }
        public string DDSendId { get; set; }
        public string DefaultSysCode { get; set; }
    }
}
