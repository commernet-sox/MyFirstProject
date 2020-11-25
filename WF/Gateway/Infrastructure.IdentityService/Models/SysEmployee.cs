using CPC;
using System;
using System.Collections.Generic;

namespace Infrastructure.IdentityService.Models
{
    public partial class SysEmployee : IMapEntity
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public ulong IsFired { get; set; }
        public string Idcard { get; set; }
        public int Sex { get; set; }
        public DateTime? EnterDate { get; set; }
        public string CompanyId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Job { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public string Memo { get; set; }
        public string DepartmentId { get; set; }
        public DateTime? LeaveDate { get; set; }
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
