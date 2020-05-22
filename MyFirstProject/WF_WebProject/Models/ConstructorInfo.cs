using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WFWebProject.Models
{
    public class ConstructorInfo
    {
        public int Id { get; set; }
        //省份
        public string Province { get; set; }
        //企业名称
        public string CompanyName { get; set; }
        //建造师
        public string Constructor { get; set; }
        //注册号
        public string RegisterNumber { get; set; }
        //执业印章号
        public string PracticeSealNo { get; set; }
        //注册证书编号
        public string RegisterCertNo { get; set; }
        //执业资格证书编号
        public string QualificationCertNo { get; set; }
        //发证日期
        public string DateIssue { get; set; }
        //注册专业
        public string RegisterMajor { get; set; }
        //注册有效期
        public string ValidityRegistration { get; set; }
    }
}
