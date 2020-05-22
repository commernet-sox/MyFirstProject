using Core.Infrastructure.DataTables.Attributes;
using Core.Infrastructure.Specification;
using Core.WebServices.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WFWebProject.DTO
{
    public class CompanyQualificationDTO: BaseDTO
    {
        public int Id { get; set; }
        //企业编号
        [MappingExpression(PropertyName = "Code", DefaultOperator = ExpressionOperator.Contains)]
        public string Code { get; set; }
        //企业名称
        [MappingExpression(PropertyName = "Name", DefaultOperator = ExpressionOperator.Contains)]
        public string Name { get; set; }
        //经济类型
        [MappingExpression(PropertyName = "EconomicType", DefaultOperator = ExpressionOperator.Contains)]
        public string EconomicType { get; set; }
        //[注册地（省/市）]
        [MappingExpression(PropertyName = "Province", DefaultOperator = ExpressionOperator.Contains)]
        public string Province { get; set; }
        //[注册地（市/区）]
        [MappingExpression(PropertyName = "City", DefaultOperator = ExpressionOperator.Contains)]
        public string City { get; set; }
        //时间
        [MappingExpression(PropertyName = "Time", DefaultOperator = ExpressionOperator.Contains)]
        public string Time { get; set; }
        //邮箱
        [MappingExpression(PropertyName = "Email", DefaultOperator = ExpressionOperator.Contains)]
        public string Email { get; set; }
        //企业网址
        [MappingExpression(PropertyName = "WebSite", DefaultOperator = ExpressionOperator.Contains)]
        public string WebSite { get; set; }
        //资质类型
        [MappingExpression(PropertyName = "QualificationType", DefaultOperator = ExpressionOperator.Contains)]
        public string QualificationType { get; set; }
        //资质联系地址
        [MappingExpression(PropertyName = "ContactAddress", DefaultOperator = ExpressionOperator.Contains)]
        public string ContactAddress { get; set; }
        //邮编
        [MappingExpression(PropertyName = "ZipCode", DefaultOperator = ExpressionOperator.Contains)]
        public string ZipCode { get; set; }
        //安全生产许可证编号
        [MappingExpression(PropertyName = "SafetyLicenseNo", DefaultOperator = ExpressionOperator.Contains)]
        public string SafetyLicenseNo { get; set; }
        //[有效期（起始）]
        [MappingExpression(PropertyName = "StartDate", DefaultOperator = ExpressionOperator.Contains)]
        public Nullable<DateTime> StartDate { get; set; }
        //[有效期（截止）]
        [MappingExpression(PropertyName = "EndDate", DefaultOperator = ExpressionOperator.Contains)]
        public Nullable<DateTime> EndDate { get; set; }
        //发证机构
        [MappingExpression(PropertyName = "IssuingAuthority", DefaultOperator = ExpressionOperator.Contains)]
        public string IssuingAuthority { get; set; }
        //安全生产许可证范围
        [MappingExpression(PropertyName = "ScopeLicense", DefaultOperator = ExpressionOperator.Contains)]
        public string ScopeLicense { get; set; }
        //组织机构代码
        [MappingExpression(PropertyName = "OrganizationCode", DefaultOperator = ExpressionOperator.Contains)]
        public string OrganizationCode { get; set; }
        //信用综合评分
        [MappingExpression(PropertyName = "ComprehensiveScore", DefaultOperator = ExpressionOperator.Contains)]
        public string ComprehensiveScore { get; set; }
    }
}
