using Core.Infrastructure.DataTables.Attributes;
using Core.Infrastructure.Specification;
using Core.WebServices.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WFWebProject.DTO
{
    public class UserDTO:BaseDTO
    {
        public int Id { get; set; }
        //用户名
        [MappingExpression(PropertyName = "UserName", DefaultOperator = ExpressionOperator.Contains)]
        public string UserName { get; set; }
        //手机号
        [MappingExpression(PropertyName = "Tel", DefaultOperator = ExpressionOperator.Contains)]
        public string Tel { get; set; }
        //密码
        public string Pwd { get; set; }
    }
}
