using Core.Infrastructure.DataTables.Attributes;
using Core.Infrastructure.Specification;
using Core.WebServices.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WFWebProject.DTO
{
    public class ConstructorInfoDTO:BaseDTO
    {
        public int Id { get; set; }
        //省份
        [MappingExpression(PropertyName = "Province", DefaultOperator = ExpressionOperator.Contains)]
        public string Province { get; set; }
        //企业名称
        [MappingExpression(PropertyName = "CompanyName", DefaultOperator = ExpressionOperator.Contains)]
        public string CompanyName { get; set; }
        //建造师
        [MappingExpression(PropertyName = "Constructor", DefaultOperator = ExpressionOperator.Contains)]
        public string Constructor { get; set; }
        //注册号
        [MappingExpression(PropertyName = "RegisterNumber", DefaultOperator = ExpressionOperator.Contains)]
        public string RegisterNumber { get; set; }
        //执业印章号
        [MappingExpression(PropertyName = "PracticeSealNo", DefaultOperator = ExpressionOperator.Contains)]
        public string PracticeSealNo { get; set; }
        //注册证书编号
        [MappingExpression(PropertyName = "RegisterCertNo", DefaultOperator = ExpressionOperator.Contains)]
        public string RegisterCertNo { get; set; }
        //执业资格证书编号
        [MappingExpression(PropertyName = "QualificationCertNo", DefaultOperator = ExpressionOperator.Contains)]
        public string QualificationCertNo { get; set; }
        //发证日期
        [MappingExpression(PropertyName = "DateIssue", DefaultOperator = ExpressionOperator.GreaterThanOrEqual)]
        public string DateIssue { get; set; }
        //发证日期
        [MappingExpression(PropertyName = "DateIssue", DefaultOperator = ExpressionOperator.LessThanOrEqual)]
        public string DateIssue1 { get; set; }
        //注册专业
        [MappingExpression(PropertyName = "RegisterMajor", DefaultOperator = ExpressionOperator.Contains)]
        public string RegisterMajor { get; set; }
        //注册有效期
        [MappingExpression(PropertyName = "ValidityRegistration", DefaultOperator = ExpressionOperator.Contains)]
        public string ValidityRegistration { get; set; }
        //备注
        [MappingExpression(PropertyName = "Remarks", DefaultOperator = ExpressionOperator.Contains)]
        public string Remarks { get; set; }
        //修改时间
        public Nullable<DateTime> ModifyTime { get; set; }
        //修改人
        public string Modifier { get; set; }
    }
}
