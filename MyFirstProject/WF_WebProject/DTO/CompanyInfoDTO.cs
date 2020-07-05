using Core.Infrastructure.DataTables.Attributes;
using Core.Infrastructure.Specification;
using Core.WebServices.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WFWebProject.DTO
{
    public class CompanyInfoDTO:BaseDTO
    {
        public System.Int32 Id { get; set; }
        //所属省份
        [MappingExpression(PropertyName = "Province", DefaultOperator = ExpressionOperator.Contains)]
        public System.String Province { get; set; }
        //所属市区
        [MappingExpression(PropertyName = "City", DefaultOperator = ExpressionOperator.Contains)]
        public System.String City { get; set; }
        //所属区县
        [MappingExpression(PropertyName = "District", DefaultOperator = ExpressionOperator.Contains)]
        public System.String District { get; set; }
        //公司类型
        [MappingExpression(PropertyName = "Type", DefaultOperator = ExpressionOperator.Contains)]
        public System.String Type { get; set; }
        //行业
        [MappingExpression(PropertyName = "Industry", DefaultOperator = ExpressionOperator.Contains)]
        public System.String Industry { get; set; }
        //公司名称
        [MappingExpression(PropertyName = "Name", DefaultOperator = ExpressionOperator.Contains)]
        public System.String Name { get; set; }
        //法定代表人
        [MappingExpression(PropertyName = "LegalPerson", DefaultOperator = ExpressionOperator.Contains)]
        public System.String LegalPerson { get; set; }
        //成立日期
        [MappingExpression(PropertyName = "CreateDate", DefaultOperator = ExpressionOperator.GreaterThanOrEqual)]
        public DateTime? CreateDate { get; set; }
        //成立日期
        
        //[MappingExpression(PropertyName = "CreateDate", DefaultOperator = ExpressionOperator.LessThanOrEqual)]
        //public DateTime? CreateDate1 { get; set; }
        //办公地址
        [MappingExpression(PropertyName = "Address", DefaultOperator = ExpressionOperator.Contains)]
        public System.String Address { get; set; }
        //企业公示的联系电话
        [MappingExpression(PropertyName = "Tel", DefaultOperator = ExpressionOperator.Contains)]
        public System.String Tel { get; set; }
        //企业公示的联系电话（更多号码）
        [MappingExpression(PropertyName = "PublicTel", DefaultOperator = ExpressionOperator.Contains)]
        public System.String PublicTel { get; set; }
        //企业公示的地址
        [MappingExpression(PropertyName = "PublicAddress", DefaultOperator = ExpressionOperator.Contains)]
        public System.String PublicAddress { get; set; }
        //企业公示的网址
        [MappingExpression(PropertyName = "PublicWebSite", DefaultOperator = ExpressionOperator.Contains)]
        public System.String PublicWebSite { get; set; }
        //企业公示的邮箱
        [MappingExpression(PropertyName = "PublicEmail", DefaultOperator = ExpressionOperator.Contains)]
        public System.String PublicEmail { get; set; }
        //经营范围
        [MappingExpression(PropertyName = "BusinessScope", DefaultOperator = ExpressionOperator.Contains)]
        public System.String BusinessScope { get; set; }
        //注册资本
        [MappingExpression(PropertyName = "RegisteredCapital", DefaultOperator = ExpressionOperator.Contains)]
        public System.String RegisteredCapital { get; set; }
        //实缴资本
        [MappingExpression(PropertyName = "PaidCapital", DefaultOperator = ExpressionOperator.Contains)]
        public System.String PaidCapital { get; set; }
        //经营状态
        [MappingExpression(PropertyName = "BusinessStatus", DefaultOperator = ExpressionOperator.Contains)]
        public System.String BusinessStatus { get; set; }
        //统一社会信用代码
        [MappingExpression(PropertyName = "CreditCode", DefaultOperator = ExpressionOperator.Contains)]
        public System.String CreditCode { get; set; }
        //工商注册号
        [MappingExpression(PropertyName = "RegistrationNo", DefaultOperator = ExpressionOperator.Contains)]
        public System.String RegistrationNo { get; set; }
        //纳税人识别号
        [MappingExpression(PropertyName = "IdentificationNumber", DefaultOperator = ExpressionOperator.Contains)]
        public System.String IdentificationNumber { get; set; }
        //组织机构代码
        [MappingExpression(PropertyName = "OrganizationCode", DefaultOperator = ExpressionOperator.Contains)]
        public System.String OrganizationCode { get; set; }
        //登记机关
        [MappingExpression(PropertyName = "RegistrationAuthority", DefaultOperator = ExpressionOperator.Contains)]
        public System.String RegistrationAuthority { get; set; }
        //营业期限
        [MappingExpression(PropertyName = "BusinessTerm", DefaultOperator = ExpressionOperator.Contains)]
        public string BusinessTerm { get; set; }


        //纳税人资质
        [MappingExpression(PropertyName = "TaxpayerQualification", DefaultOperator = ExpressionOperator.Contains)]
        public System.String TaxpayerQualification { get; set; }
        //人员规模
        [MappingExpression(PropertyName = "PersonnelSize", DefaultOperator = ExpressionOperator.Contains)]
        public System.String PersonnelSize { get; set; }
        //参保人数
        [MappingExpression(PropertyName = "NumberInsured", DefaultOperator = ExpressionOperator.Contains)]
        public System.String NumberInsured { get; set; }
        //曾用名
        [MappingExpression(PropertyName = "NameUsedBefore", DefaultOperator = ExpressionOperator.Contains)]
        public System.String NameUsedBefore { get; set; }
        //英文名称
        [MappingExpression(PropertyName = "EnglishName", DefaultOperator = ExpressionOperator.Contains)]
        public System.String EnglishName { get; set; }
        //注册地址
        [MappingExpression(PropertyName = "RegisterAddress", DefaultOperator = ExpressionOperator.Contains)]
        public System.String RegisterAddress { get; set; }


        //企业编号
        [MappingExpression(PropertyName = "Code", DefaultOperator = ExpressionOperator.Contains)]
        public string Code { get; set; }
        //企业名称
        //[MappingExpression(PropertyName = "Name", DefaultOperator = ExpressionOperator.Contains)]
        //public string Name { get; set; }
        //经济类型
        [MappingExpression(PropertyName = "EconomicType", DefaultOperator = ExpressionOperator.Contains)]
        public string EconomicType { get; set; }
        //[注册地（省/市）]
        [MappingExpression(PropertyName = "Province", DefaultOperator = ExpressionOperator.Contains)]
        public string Province1 { get; set; }
        //[注册地（市/区）]
        [MappingExpression(PropertyName = "City", DefaultOperator = ExpressionOperator.Contains)]
        public string City1 { get; set; }
        //时间
        [MappingExpression(PropertyName = "Time", DefaultOperator = ExpressionOperator.GreaterThanOrEqual)]
        public DateTime? Time { get; set; }
        //时间
        [MappingExpression(PropertyName = "Time1", DefaultOperator = ExpressionOperator.LessThanOrEqual)]
        public DateTime? Time1 { get; set; }
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
        [MappingExpression(PropertyName = "StartDate", DefaultOperator = ExpressionOperator.GreaterThanOrEqual)]
        public Nullable<DateTime> StartDate { get; set; }
        //[有效期（起始）]
        [MappingExpression(PropertyName = "StartDate1", DefaultOperator = ExpressionOperator.LessThanOrEqual)]
        public Nullable<DateTime> StartDate1 { get; set; }
        //[有效期（截止）]
        [MappingExpression(PropertyName = "EndDate", DefaultOperator = ExpressionOperator.GreaterThanOrEqual)]
        public Nullable<DateTime> EndDate { get; set; }
        //[有效期（截止）]
        [MappingExpression(PropertyName = "EndDate1", DefaultOperator = ExpressionOperator.LessThanOrEqual)]
        public Nullable<DateTime> EndDate1 { get; set; }
        //发证机构
        [MappingExpression(PropertyName = "IssuingAuthority", DefaultOperator = ExpressionOperator.Contains)]
        public string IssuingAuthority { get; set; }
        //安全生产许可证范围
        [MappingExpression(PropertyName = "ScopeLicense", DefaultOperator = ExpressionOperator.Contains)]
        public string ScopeLicense { get; set; }
        //组织机构代码
        [MappingExpression(PropertyName = "OrganizationCode", DefaultOperator = ExpressionOperator.Contains)]
        public string OrganizationCode1 { get; set; }
        //信用综合评分
        [MappingExpression(PropertyName = "ComprehensiveScore", DefaultOperator = ExpressionOperator.Contains)]
        public string ComprehensiveScore { get; set; }

        //备注
        [MappingExpression(PropertyName = "Remarks", DefaultOperator = ExpressionOperator.Contains)]
        public string Remarks { get; set; }
        //修改时间
        [MappingExpression(PropertyName = "ModifyTime", DefaultOperator = ExpressionOperator.Contains)]
        public Nullable<DateTime> ModifyTime { get; set; }
        //修改人
        [MappingExpression(PropertyName = "Modifier", DefaultOperator = ExpressionOperator.Contains)]
        public string Modifier { get; set; }
    }
}
