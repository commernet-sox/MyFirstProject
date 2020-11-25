using CPC;
using System;
using System.Collections.Generic;

namespace Data.IdentityService
{
    public partial class SysDepartmentDTO : IMapDto
    {
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string CompanyId { get; set; }
        public string ParentId { get; set; }
        public int? ShowOrder { get; set; }
        public bool? Status { get; set; }
        public string Manager { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string Memo { get; set; }
        public string Ex1 { get; set; }
        public string Ex2 { get; set; }
        public int? Ex3 { get; set; }
        public int? Ex4 { get; set; }
    }
}
