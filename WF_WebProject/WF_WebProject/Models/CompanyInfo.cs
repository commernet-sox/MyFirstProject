using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WFWebProject.Models
{
    public class CompanyInfo
    {
        public int Id { get; set; }
        //所属省份
        public string Province { get; set; }
        //所属市区
        public string City { get; set; }
        //所属区县
        public string District { get; set; }
        //公司类型
        public string Type { get; set; }
        //行业
        public string Industry { get; set; }
        //公司名称
        public string Name { get; set; }
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
        //BusinessStatus
        public string BusinessStatus { get; set; }
        //统一社会信用代码
        public string CreditCode { get; set; }
        //工商注册号
        public string RegistrationNo { get; set; }
        //纳税人识别号
        public string IdentificationNumber { get; set; }
        //组织机构代码
        public string OrganizationCode { get; set; }
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
