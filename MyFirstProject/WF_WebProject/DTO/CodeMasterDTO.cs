using Core.WebServices.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Infrastructure.DataTables.Attributes;
using Core.Infrastructure.Specification;

namespace WFWebProject.DTO
{
    public class CodeMasterDTO:BaseDTO
    {
        public System.Int32 Id { get; set; }
        [MappingExpression(PropertyName = "ModifyTime", DefaultOperator = ExpressionOperator.GreaterThanOrEqual)]
        public Nullable<DateTime> ModifyTime { get; set; }
        [MappingExpression(PropertyName = "Modifier", DefaultOperator = ExpressionOperator.Contains)]
        public System.String Modifier { get; set; }
        [MappingExpression(PropertyName = "CreateTime", DefaultOperator = ExpressionOperator.GreaterThanOrEqual)]
        public System.DateTime CreateTime { get; set; }
        [MappingExpression(PropertyName = "Creator", DefaultOperator = ExpressionOperator.Contains)]
        public System.String Creator { get; set; }
        [MappingExpression(PropertyName = "CodeGroup", DefaultOperator = ExpressionOperator.Contains)]
        public System.String CodeGroup { get; set; }
        [MappingExpression(PropertyName = "CodeId", DefaultOperator = ExpressionOperator.Contains)]
        public System.String CodeId { get; set; }
        [MappingExpression(PropertyName = "CodeName", DefaultOperator = ExpressionOperator.Contains)]
        public System.String CodeName { get; set; }
        [MappingExpression(PropertyName = "IsActive", DefaultOperator = ExpressionOperator.Equal)]
        public System.Char IsActive { get; set; }
        [MappingExpression(PropertyName = "Remarks", DefaultOperator = ExpressionOperator.Contains)]
        public System.String Remarks { get; set; }
        [MappingExpression(PropertyName = "HUDF_01", DefaultOperator = ExpressionOperator.Contains)]
        public System.String HUDF_01 { get; set; }
    }
}
