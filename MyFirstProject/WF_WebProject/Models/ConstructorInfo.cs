using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WFWebProject.Models
{
    /// <summary>
    /// 建造师信息表
    /// </summary>
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
        public DateTime? DateIssue { get; set; }
        //注册专业
        public string RegisterMajor { get; set; }
        //注册有效期
        public DateTime? ValidityRegistration { get; set; }
        //备注,跟进记录
        public string Remarks { get; set; }
        //修改时间，跟进时间
        public Nullable<DateTime> ModifyTime { get; set; }
        //修改人，跟进人
        public string Modifier { get; set; }
        //职称
        public string Thetitle { get; set; }
        //执业证书
        public string Practicecertificate { get; set; }
        //注册状况
        public string Registrationstatus { get; set; }
        //性别
        public string Sex { get; set; }
        //出生年月
        public string Birthmonth { get; set; }
        //身份证号
        public string Idcard { get; set; }
        //学历
        public string Educationbackground { get; set; }
        //专业
        public string Major { get; set; }
        //毕业院校
        public string GraduateSchool { get; set; }
        //工作年限
        public string Workyear { get; set; }
        //所在地区
        public string Location { get; set; }
        //手机
        public string Mobile { get; set; }
        //微信
        public string Wechat { get; set; }
        //邮箱
        public string Email { get; set; }
        //qq
        public string QQ { get; set; }
        //求职省份
        public string Jobprovinces { get; set; }
        //求职性质
        public string Jobnature { get; set; }
        //证书状态
        public string Certificatestatus { get; set; }
        //期望薪资
        public string Expectedsalary { get; set; }
        //是否可以转社保
        public string Socialsecurity { get; set; }
        //求职说明
        public string Jobapply { get; set; }
    }
}
