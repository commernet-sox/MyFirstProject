using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WFWebProject.Models
{
    public class CompanyQualification
    {
        public int Id { get; set; }
        //企业编号
        public string Code { get; set; }
        //企业名称
        public string Name { get; set; }
        //经济类型
        //public string EconomicType { get; set; }
        //[注册地（省/市）]所属省份
        public string Province { get; set; }
        //[注册地（市/区）]所属市区
        public string City { get; set; }
        //时间
        //public DateTime Time { get; set; }
        //邮箱
        //public string Email { get; set; }
        //企业网址
        //public string WebSite { get; set; }
        //资质类型
        public string QualificationType { get; set; }
        //资质联系地址
        //public string ContactAddress { get; set; }
        //邮编
        //public string ZipCode { get; set; }
        //安全生产许可证编号
        public string SafetyLicenseNo { get; set; }
        //[有效期（起始）]资质生效日期
        public Nullable<DateTime> StartDate { get; set; }
        //[有效期（截止）]资质到期日期
        public Nullable<DateTime> EndDate { get; set; }
        //发证机构
        public string IssuingAuthority { get; set; }
        //安全生产许可证范围
        public string ScopeLicense { get; set; }
        //组织机构代码
        public string OrganizationCode { get; set; }
        //信用综合评分
        //public string ComprehensiveScore { get; set; }
        //备注
        public string Remarks { get; set; }
        //修改时间
        public Nullable<DateTime> ModifyTime { get; set; }
        //修改人
        public string Modifier { get; set; }
        //二级资质细分
        public string SecondQualificationDetail { get; set; }
        //三级资质细分
        public string ThirdQualificationDetail { get; set; }
        //资质等级
        public string QualificationLevel { get; set; }
        //资质编号
        public string QualificationNumber { get; set; }
        //安许证生效日期
        public Nullable<DateTime> SafetyLicenseStartTime { get; set; }
        //安许证到期日期
        public Nullable<DateTime> SafetyLicenseEndTime { get; set; }
        //所属区县
        public string County { get; set; }
        //公司类型
        public string CompanyType { get; set; }
        //所属行业
        public string Industry { get; set; }
        //公司名称
        public string CompanyName { get; set; }
        //法定代表人
        public string LegalPerson { get; set; }
        //成立日期
        public string CreateDate { get; set; }
        //办公地址
        public string Address { get; set; }
        //企业公示的联系电话
        public string Tel { get; set; }
        //企业公示的联系电话（更多号码）
        public string PublicTel { get; set; }
        //企业公示的地址
        public string PublicAddress { get; set; }
        //企业公示的网址
        public string PublicWebSite { get; set; }
        //企业公示的邮箱
        public string PublicEmail { get; set; }
        //经营范围
        public string BusinessScope { get; set; }
        //注册资本
        public string RegisteredCapital { get; set; }
        //实缴资本
        public string PaidCapital { get; set; }
        //经营状态
        public string BusinessStatus { get; set; }
        //统一社会信用代码
        public string CreditCode { get; set; }
        //工商注册号
        public string RegistrationNo { get; set; }
        //纳税人识别号
        public string IdentificationNumber { get; set; }
        //登记机关
        public string RegistrationAuthority { get; set; }
        //营业期限
        public string BusinessTerm { get; set; }
        //纳税人资质
        public string TaxpayerQualification { get; set; }
        //人员规模
        public string PersonnelSize { get; set; }
        //参保人数
        public string NumberInsured { get; set; }
        //曾用名
        public string NameUsedBefore { get; set; }
        //英文名称
        public string EnglishName { get; set; }
        //注册地址
        public string RegisterAddress { get; set; }

    }
}
