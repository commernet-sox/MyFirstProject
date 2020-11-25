using CPC;
using System;
using System.Collections.Generic;

namespace Data.IdentityService
{
    public partial class SysRoleDTO : IMapDto
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool? IsSysRole { get; set; }
        public string SysCode { get; set; }
        public bool? Status { get; set; }
        public string Ex1 { get; set; }
        public string Ex2 { get; set; }
        public int? Ex3 { get; set; }
        public int? Ex4 { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
