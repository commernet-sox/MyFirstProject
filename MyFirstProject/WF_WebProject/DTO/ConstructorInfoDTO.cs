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
        public DateTime? DateIssue { get; set; }
        //发证日期
        [AutoMapper.IgnoreMap]
        [MappingExpression(PropertyName = "DateIssue", DefaultOperator = ExpressionOperator.LessThanOrEqual)]
        public DateTime? DateIssue1 { get; set; }
        //注册专业
        [MappingExpression(PropertyName = "RegisterMajor", DefaultOperator = ExpressionOperator.Contains)]
        public string RegisterMajor { get; set; }
        //注册有效期
        [MappingExpression(PropertyName = "ValidityRegistration", DefaultOperator = ExpressionOperator.GreaterThanOrEqual)]
        public DateTime? ValidityRegistration { get; set; }
        //注册有效期
        [AutoMapper.IgnoreMap]
        [MappingExpression(PropertyName = "ValidityRegistration", DefaultOperator = ExpressionOperator.LessThanOrEqual)]
        public DateTime? ValidityRegistration1 { get; set; }
        //备注
        [MappingExpression(PropertyName = "Remarks", DefaultOperator = ExpressionOperator.Contains)]
        public string Remarks { get; set; }
        //修改时间
        public Nullable<DateTime> ModifyTime { get; set; }
        //修改人
        public string Modifier { get; set; }
        //职称
        [MappingExpression(PropertyName = "Thetitle", DefaultOperator = ExpressionOperator.Contains)]
        public string Thetitle { get; set; }
        //执业证书
        [MappingExpression(PropertyName = "Practicecertificate", DefaultOperator = ExpressionOperator.Contains)]
        public string Practicecertificate { get; set; }
        //注册状况
        [MappingExpression(PropertyName = "Registrationstatus", DefaultOperator = ExpressionOperator.Contains)]
        public string Registrationstatus { get; set; }
        //性别
        [MappingExpression(PropertyName = "Sex", DefaultOperator = ExpressionOperator.Contains)]
        public string Sex { get; set; }
        //出生年月
        [MappingExpression(PropertyName = "Birthmonth", DefaultOperator = ExpressionOperator.Contains)]
        public string Birthmonth { get; set; }
        //身份证号
        [MappingExpression(PropertyName = "Idcard", DefaultOperator = ExpressionOperator.Contains)]
        public string Idcard { get; set; }
        //学历
        [MappingExpression(PropertyName = "Educationbackground", DefaultOperator = ExpressionOperator.Contains)]
        public string Educationbackground { get; set; }
        //专业
        [MappingExpression(PropertyName = "Major", DefaultOperator = ExpressionOperator.Contains)]
        public string Major { get; set; }
        //毕业院校
        [MappingExpression(PropertyName = "GraduateSchool", DefaultOperator = ExpressionOperator.Contains)]
        public string GraduateSchool { get; set; }
        //工作年限
        [MappingExpression(PropertyName = "Workyear", DefaultOperator = ExpressionOperator.Contains)]
        public string Workyear { get; set; }
        //所在地区
        [MappingExpression(PropertyName = "Location", DefaultOperator = ExpressionOperator.Contains)]
        public string Location { get; set; }
        //手机
        [MappingExpression(PropertyName = "Mobile", DefaultOperator = ExpressionOperator.Contains)]
        public string Mobile { get; set; }
        //微信
        [MappingExpression(PropertyName = "Wechat", DefaultOperator = ExpressionOperator.Contains)]
        public string Wechat { get; set; }
        //邮箱
        [MappingExpression(PropertyName = "Email", DefaultOperator = ExpressionOperator.Contains)]
        public string Email { get; set; }
        //qq
        [MappingExpression(PropertyName = "QQ", DefaultOperator = ExpressionOperator.Contains)]
        public string QQ { get; set; }
        //求职省份
        [MappingExpression(PropertyName = "Jobprovinces", DefaultOperator = ExpressionOperator.Contains)]
        public string Jobprovinces { get; set; }
        //求职性质
        [MappingExpression(PropertyName = "Jobnature", DefaultOperator = ExpressionOperator.Contains)]
        public string Jobnature { get; set; }
        //证书状态
        [MappingExpression(PropertyName = "Certificatestatus", DefaultOperator = ExpressionOperator.Contains)]
        public string Certificatestatus { get; set; }
        //期望薪资
        [MappingExpression(PropertyName = "Expectedsalary", DefaultOperator = ExpressionOperator.Contains)]
        public string Expectedsalary { get; set; }
        //是否可以转社保
        [MappingExpression(PropertyName = "Socialsecurity", DefaultOperator = ExpressionOperator.Contains)]
        public string Socialsecurity { get; set; }
        //求职说明
        [MappingExpression(PropertyName = "Jobapply", DefaultOperator = ExpressionOperator.Contains)]
        public string Jobapply { get; set; }
    }
}
