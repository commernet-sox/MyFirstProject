using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace WcfService1
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [JavascriptCallbackBehavior(UrlParameterName = "jsoncallback")]
    public class Service1 : IService1
    {
        public string GetData(string value)
        {
            return string.Format("You Get entered: {0}", value);
        }

        public string PostData(string value)
        {
            return string.Format("You Post entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        //.net core MailKit发送
        public string LeavingMessage(string Name, string Tel, string Email, string OtherWay, string Message)
        {
            //Console.WriteLine(Name+Tel+Email+OtherWay+Message);
            return WcfService1.Email.LeavingMessage(Name, Tel, Email, OtherWay, Message);
        }

        // System.Net.Mail 在服务器部署发送失败
        public string SendEmail(string Name, string Tel, string Email, string OtherWay, string Message)
        {
            return WcfService1.Email.Sendmail(Name, Tel, Email, OtherWay, Message);
        }
        /// <summary>
        /// 获取资质信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetGetStandardDetailById(string id)
        {
            try
            {
                string sql = " select * from dbo.Standard where Id='{0}'";
                sql = string.Format(sql,id);
                DataTable dt = SqlHelpClass.GetReadOnlyDataTable(sql);
                List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
                Dictionary<string, string> dic = new Dictionary<string, string>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        
                        dic.Add("Id", dr["Id"].ToString());
                        dic.Add("Title", dr["Title"].ToString());
                        dic.Add("Description", dr["Description"].ToString());
                        dic.Add("Content", dr["Content"].ToString());
                        dic.Add("Type", dr["Type"].ToString());
                        dic.Add("Img", "");
                        dic.Add("CreateTime", dr["CreateTime"].ToString());
                        list.Add(dic);
                    }
                    return JsonConvert.SerializeObject(dic);
                }
                else
                {
                    return JsonConvert.SerializeObject("");
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.Message);
            }

            //Dictionary<string, string> dic = new Dictionary<string, string>();
            //dic.Add("Title", "建筑工程施工总承包");
            //dic.Add("Description", "【摘要】：建筑工程施工总承包资质分为特级、一级、二级、三级。");
            //dic.Add("Content", "建筑工程施工总承包资质分为特级、一级、二级、三级" +
            //    "特级资质标准：" +
            //    "一、企业资信能力" +
            //    "1、企业注册资本金3亿元以上。" +
            //    "2、企业净资产3.6亿元以上。" +
            //    "3、企业近三年上缴建筑业营业税均在5000万元以上。" +
            //    "4、企业银行授信额度近三年均在5亿元以上。");
            //return Newtonsoft.Json.JsonConvert.SerializeObject(dic);
        }

        public string CreateStandard(string Id, string Title, string Description, string Content, string Img, string Type, string CreateTime)
        {
            try
            {
                string sql = " INSERT INTO dbo.Standard (Title,Description,Content,Type,CreateTime) VALUES ('{0}','{1}','{2}','{3}','" + DateTime.Now + "') ";
                sql = string.Format(sql,Title,Description,Content,Type);
                int res = SqlHelpClass.ExecuteNonQueryTypeText(sql);
                if (res > 0)
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetAllStandard()
        {
            try
            {
                string sql = " select top 10 * from dbo.Standard order by Id desc";
                DataTable dt = SqlHelpClass.GetReadOnlyDataTable(sql);
                List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("Id", dr["Id"].ToString());
                        dic.Add("Title", dr["Title"].ToString());
                        dic.Add("Description", dr["Description"].ToString());
                        dic.Add("Content", dr["Content"].ToString());
                        dic.Add("Type", dr["Type"].ToString());
                        dic.Add("Img", "");
                        dic.Add("CreateTime", dr["CreateTime"].ToString());
                        list.Add(dic);
                    }
                    return JsonConvert.SerializeObject(list);
                }
                else
                {
                    return JsonConvert.SerializeObject("");
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.Message);
            }
            
        }

        public string GetAllPolicyList()
        {
            string html = HttpRequestMiddleware.SendRequest("http://www.coc.gov.cn/coc/webview/titleList.jspx?channel=1&pageNo=1",new Dictionary<string, string>(), HttpMethod.Get,new Dictionary<string, string>(),5000);
            string list = Regex.Match(html, "var\\stitleList=JSON\\.parse\\(\'.*").Value;
            list = Regex.Replace(list, "var\\stitleList=JSON\\.parse\\(\'","");
            list = Regex.Replace(list,"\'\\);","");
            return list;
        }
    }
    public class Email
    {
        public static string Sendmail(string Name, string Tel, string Email, string OtherWay, string Message)
        {
            //try
            //{
            //    //Console.WriteLine("用户名");
            //    string uid = "1510595785@qq.com";//发件人邮箱地址@符号前面的字符tom@dddd.com,则为"tom"  
            //                                     //Console.WriteLine("密码");//发件人邮箱的密码  
            //    string pwd = "fpzmnaizbbckgeff";//qwertyuiop...
            //    string ReceiveAddress = "209045565@qq.com";
            //    for (int i = 0; i < 1; i++) //连发20封，嘿嘿  
            //    {
            //        MailAddress from = new MailAddress(uid);
            //        MailAddress to = new MailAddress(ReceiveAddress);
            //        MailMessage mailMessage = new MailMessage(from, to);
            //        mailMessage.Subject = Name + " 给您留言";//邮件主题  
            //        mailMessage.Body = "电话:" + Tel + "\n" + "邮箱:" + Email + "\n" + "其他联系方式:" + OtherWay + "\n" + "留言内容:" + Message;//邮件正文  

            //        //添加附件  

            //        //string file1 = "test.txt";
            //        //Attachment attachment1 = new Attachment(file1, MediaTypeNames.Text.Plain);
            //        ////为附件添加时间信息  
            //        //ContentDisposition disposition1 = attachment1.ContentDisposition;
            //        //disposition1.CreationDate = File.GetCreationTime(file1);
            //        //disposition1.ModificationDate = File.GetLastWriteTime(file1);
            //        //disposition1.ReadDate = File.GetLastAccessTime(file1);
            //        //mailMessage.Attachments.Add(attachment1);

            //        //string file2 = "test.doc";
            //        //Attachment attachment2 = new Attachment(file2);
            //        ////为附件添加时间信息  
            //        //ContentDisposition disposition2 = attachment2.ContentDisposition;
            //        //disposition2.CreationDate = File.GetCreationTime(file2);
            //        //disposition2.ModificationDate = File.GetLastWriteTime(file2);
            //        //disposition2.ReadDate = File.GetLastAccessTime(file2);
            //        //mailMessage.Attachments.Add(attachment2);

            //        //实例化SmtpClient  
            //        SmtpClient smtpClient = new SmtpClient("smtp.qq.com",25);
            //        //设置验证发件人身份的凭据  
            //        smtpClient.Credentials = new NetworkCredential(uid, pwd);
            //        //发送  
            //        smtpClient.Send(mailMessage);

            //        //Console.WriteLine("OK - [{0}]", i);

            //    }
            //    return "OK";
            //    //Console.ReadKey();
            //}
            //catch (Exception ex)
            //{
            //    return ex.Message;
            //}
            return "弃用";

        }

        public static string LeavingMessage(string Name, string Tel, string Email, string OtherWay, string Message)
        {
            try
            {
                string host = "smtp.qq.com";//SMTP服务器
                int port = 465;//25 465 587
                string from = "1510595785@qq.com";//发件人qq
                string to = "209045565@qq.com";//qq小号
                string to1 = "746867748@qq.com";//公司qq
                string userName = "1510595785@qq.com";//SMTP服务账号
                string password = "fpzmnaizbbckgeff";//SMTP服务验证密码

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(from));
                message.To.AddRange(new MailboxAddress[] { new MailboxAddress(to) });
                //message.To.AddRange(new MailboxAddress[] { new MailboxAddress(to),new MailboxAddress(to1) });
                message.Subject = Name + " 给您留言";
                var entity = new TextPart(TextFormat.Text)
                {
                    Text = "<电话:>" + Tel + "\n" + "<邮箱:>" + Email + "\n" + "<其他联系方式:>" + OtherWay + "\n" + "<留言内容:>" + Message
                };
                message.Body = entity;
                SmtpClient client = new SmtpClient();
                client.Connect(host, port, port == 465);//465端口是ssl端口
                client.Authenticate(userName, password);
                client.Send(message);
                client.Disconnect(true);
                return "留言成功!";
            }
            catch (Exception ex)
            {
                return "网络延迟，请稍后再试，或联系我们!";
            }
        }

    }
}
