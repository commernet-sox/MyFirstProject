using System;
using System.Collections.Generic;
using System.Text;

namespace Data.IdentityService.Model
{
    public class UserModel:BaseModel
    {
        public string UserId { get; set; }
        public string UserPwd { get; set; }
        public string EmployeeId { get; set; }
        public string CompanyId { get; set; }
        public string PersonName { get; set; }
    }
}
