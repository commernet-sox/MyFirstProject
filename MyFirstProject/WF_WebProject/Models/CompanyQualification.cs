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
        public string EconomicType { get; set; }
        //[注册地（省/市）]
        public string Province { get; set; }
        //[注册地（市/区）]
        public string City { get; set; }
        //时间
        public DateTime Time { get; set; }
        //邮箱
        public string Email { get; set; }
        //企业网址
        public string WebSite { get; set; }
        //资质类型
        public string QualificationType { get; set; }
        //资质联系地址
        public string ContactAddress { get; set; }
        //邮编
        public string ZipCode { get; set; }
        //安全生产许可证编号
        public string SafetyLicenseNo { get; set; }
        //[有效期（起始）]
        public Nullable<DateTime> StartDate { get; set; }
        //[有效期（截止）]
        public Nullable<DateTime> EndDate { get; set; }
        //发证机构
        public string IssuingAuthority { get; set; }
        //安全生产许可证范围
        public string ScopeLicense { get; set; }
        //组织机构代码
        public string OrganizationCode { get; set; }
        //信用综合评分
        public string ComprehensiveScore { get; set; }
        //备注
        public string Remarks { get; set; }
        //修改时间
        public Nullable<DateTime> ModifyTime { get; set; }
        //修改人
        public string Modifier { get; set; }
    }
}
