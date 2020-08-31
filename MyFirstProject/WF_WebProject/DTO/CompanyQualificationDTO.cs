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
        //[MappingExpression(PropertyName = "EconomicType", DefaultOperator = ExpressionOperator.Contains)]
        //public string EconomicType { get; set; }
        //[注册地（省/市）]
        [MappingExpression(PropertyName = "Province", DefaultOperator = ExpressionOperator.Contains)]
        public string Province { get; set; }
        //[注册地（市/区）]
        [MappingExpression(PropertyName = "City", DefaultOperator = ExpressionOperator.Contains)]
        public string City { get; set; }
        ////时间
        //[MappingExpression(PropertyName = "Time", DefaultOperator = ExpressionOperator.GreaterThanOrEqual)]
        //public DateTime? Time { get; set; }
        ////时间
        //[MappingExpression(PropertyName = "Time", DefaultOperator = ExpressionOperator.LessThanOrEqual)]
        //public DateTime? Time1 { get; set; }
        ////邮箱
        //[MappingExpression(PropertyName = "Email", DefaultOperator = ExpressionOperator.Contains)]
        //public string Email { get; set; }
        ////企业网址
        //[MappingExpression(PropertyName = "WebSite", DefaultOperator = ExpressionOperator.Contains)]
        //public string WebSite { get; set; }
        //资质类型
        [MappingExpression(PropertyName = "QualificationType", DefaultOperator = ExpressionOperator.Contains)]
        public string QualificationType { get; set; }
        //资质联系地址
        //[MappingExpression(PropertyName = "ContactAddress", DefaultOperator = ExpressionOperator.Contains)]
        //public string ContactAddress { get; set; }
        ////邮编
        //[MappingExpression(PropertyName = "ZipCode", DefaultOperator = ExpressionOperator.Contains)]
        //public string ZipCode { get; set; }
        //安全生产许可证编号
        [MappingExpression(PropertyName = "SafetyLicenseNo", DefaultOperator = ExpressionOperator.Contains)]
        public string SafetyLicenseNo { get; set; }
        //[有效期（起始）]
        [MappingExpression(PropertyName = "StartDate", DefaultOperator = ExpressionOperator.GreaterThanOrEqual)]
        public Nullable<DateTime> StartDate { get; set; }
        //[有效期（起始）]
        [MappingExpression(PropertyName = "StartDate", DefaultOperator = ExpressionOperator.LessThanOrEqual)]
        public Nullable<DateTime> StartDate1 { get; set; }
        //[有效期（截止）]
        [MappingExpression(PropertyName = "EndDate", DefaultOperator = ExpressionOperator.GreaterThanOrEqual)]
        public Nullable<DateTime> EndDate { get; set; }
        //[有效期（截止）]
        [MappingExpression(PropertyName = "EndDate", DefaultOperator = ExpressionOperator.LessThanOrEqual)]
        public Nullable<DateTime> EndDate1 { get; set; }
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
        //[MappingExpression(PropertyName = "ComprehensiveScore", DefaultOperator = ExpressionOperator.Contains)]
        //public string ComprehensiveScore { get; set; }

        //备注
        [MappingExpression(PropertyName = "Remarks", DefaultOperator = ExpressionOperator.Contains)]
        public string Remarks { get; set; }
        //修改时间
        public Nullable<DateTime> ModifyTime { get; set; }
        //修改人
        public string Modifier { get; set; }
        //二级资质细分
        [MappingExpression(PropertyName = "SecondQualificationDetail", DefaultOperator = ExpressionOperator.Contains)]
        public string SecondQualificationDetail { get; set; }
        //三级资质细分
        [MappingExpression(PropertyName = "ThirdQualificationDetail", DefaultOperator = ExpressionOperator.Contains)]
        public string ThirdQualificationDetail { get; set; }
        //资质等级
        [MappingExpression(PropertyName = "QualificationLevel", DefaultOperator = ExpressionOperator.Contains)]
        public string QualificationLevel { get; set; }
        //资质编号
        [MappingExpression(PropertyName = "QualificationNumber", DefaultOperator = ExpressionOperator.Contains)]
        public string QualificationNumber { get; set; }
        //安许证生效日期
        [MappingExpression(PropertyName = "SafetyLicenseStartTime", DefaultOperator = ExpressionOperator.GreaterThanOrEqual)]
        public Nullable<DateTime> SafetyLicenseStartTime { get; set; }
        //安许证生效日期1
        [MappingExpression(PropertyName = "SafetyLicenseStartTime", DefaultOperator = ExpressionOperator.LessThanOrEqual)]
        public Nullable<DateTime> SafetyLicenseStartTime1 { get; set; }
        //安许证到期日期
        [MappingExpression(PropertyName = "SafetyLicenseEndTime", DefaultOperator = ExpressionOperator.GreaterThanOrEqual)]
        public Nullable<DateTime> SafetyLicenseEndTime { get; set; }
        //安许证到期日期1
        [MappingExpression(PropertyName = "SafetyLicenseEndTime", DefaultOperator = ExpressionOperator.LessThanOrEqual)]
        public Nullable<DateTime> SafetyLicenseEndTime1 { get; set; }
        //所属区县
        [MappingExpression(PropertyName = "County", DefaultOperator = ExpressionOperator.Contains)]
        public string County { get; set; }
        //公司类型
        [MappingExpression(PropertyName = "CompanyType", DefaultOperator = ExpressionOperator.Contains)]
        public string CompanyType { get; set; }
        //所属行业
        [MappingExpression(PropertyName = "Industry", DefaultOperator = ExpressionOperator.Contains)]
        public string Industry { get; set; }
        //公司名称
        [MappingExpression(PropertyName = "CompanyName", DefaultOperator = ExpressionOperator.Contains)]
        public string CompanyName { get; set; }
        //法定代表人
        [MappingExpression(PropertyName = "LegalPerson", DefaultOperator = ExpressionOperator.Contains)]
        public string LegalPerson { get; set; }
        //成立日期
        [MappingExpression(PropertyName = "CreateDate", DefaultOperator = ExpressionOperator.Contains)]
        public string CreateDate { get; set; }
        //办公地址
        [MappingExpression(PropertyName = "Address", DefaultOperator = ExpressionOperator.Contains)]
        public string Address { get; set; }
        //企业公示的联系电话
        [MappingExpression(PropertyName = "Tel", DefaultOperator = ExpressionOperator.Contains)]
        public string Tel { get; set; }
        //企业公示的联系电话（更多号码）
        [MappingExpression(PropertyName = "PublicTel", DefaultOperator = ExpressionOperator.Contains)]
        public string PublicTel { get; set; }
        //企业公示的地址
        [MappingExpression(PropertyName = "PublicAddress", DefaultOperator = ExpressionOperator.Contains)]
        public string PublicAddress { get; set; }
        //企业公示的网址
        [MappingExpression(PropertyName = "PublicWebSite", DefaultOperator = ExpressionOperator.Contains)]
        public string PublicWebSite { get; set; }
        //企业公示的邮箱
        [MappingExpression(PropertyName = "PublicEmail", DefaultOperator = ExpressionOperator.Contains)]
        public string PublicEmail { get; set; }
        //经营范围
        [MappingExpression(PropertyName = "BusinessScope", DefaultOperator = ExpressionOperator.Contains)]
        public string BusinessScope { get; set; }
        //注册资本
        [MappingExpression(PropertyName = "RegisteredCapital", DefaultOperator = ExpressionOperator.Contains)]
        public string RegisteredCapital { get; set; }
        //实缴资本
        [MappingExpression(PropertyName = "PaidCapital", DefaultOperator = ExpressionOperator.Contains)]
        public string PaidCapital { get; set; }
        //经营状态
        [MappingExpression(PropertyName = "BusinessStatus", DefaultOperator = ExpressionOperator.Contains)]
        public string BusinessStatus { get; set; }
        //统一社会信用代码
        [MappingExpression(PropertyName = "CreditCode", DefaultOperator = ExpressionOperator.Contains)]
        public string CreditCode { get; set; }
        //工商注册号
        [MappingExpression(PropertyName = "RegistrationNo", DefaultOperator = ExpressionOperator.Contains)]
        public string RegistrationNo { get; set; }
        //纳税人识别号
        [MappingExpression(PropertyName = "IdentificationNumber", DefaultOperator = ExpressionOperator.Contains)]
        public string IdentificationNumber { get; set; }
        //登记机关
        [MappingExpression(PropertyName = "RegistrationAuthority", DefaultOperator = ExpressionOperator.Contains)]
        public string RegistrationAuthority { get; set; }
        //营业期限
        [MappingExpression(PropertyName = "BusinessTerm", DefaultOperator = ExpressionOperator.Contains)]
        public string BusinessTerm { get; set; }
        //纳税人资质
        [MappingExpression(PropertyName = "TaxpayerQualification", DefaultOperator = ExpressionOperator.Contains)]
        public string TaxpayerQualification { get; set; }
        //人员规模
        [MappingExpression(PropertyName = "PersonnelSize", DefaultOperator = ExpressionOperator.Contains)]
        public string PersonnelSize { get; set; }
        //参保人数
        [MappingExpression(PropertyName = "NumberInsured", DefaultOperator = ExpressionOperator.Contains)]
        public string NumberInsured { get; set; }
        //曾用名
        [MappingExpression(PropertyName = "NameUsedBefore", DefaultOperator = ExpressionOperator.Contains)]
        public string NameUsedBefore { get; set; }
        //英文名称
        [MappingExpression(PropertyName = "EnglishName", DefaultOperator = ExpressionOperator.Contains)]
        public string EnglishName { get; set; }
        //注册地址
        [MappingExpression(PropertyName = "RegisterAddress", DefaultOperator = ExpressionOperator.Contains)]
        public string RegisterAddress { get; set; }
    }
}
