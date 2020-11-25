using System;
using System.Collections.Generic;
using System.Text;

namespace Data.IdentityService.Model
{
    public class RoleuserModel:BaseModel
    {
        public string RoleId { get; set; }
        public string UserId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? Status { get; set; }
        public string Ex1 { get; set; }
        public string Ex2 { get; set; }
        public int? Ex3 { get; set; }
        public int? Ex4 { get; set; }
    }
}
