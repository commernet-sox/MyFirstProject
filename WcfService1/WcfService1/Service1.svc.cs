using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [JavascriptCallbackBehavior(UrlParameterName = "jsoncallback")]
    public class Service1 : IService1
    {
        public string GetData()
        {
            return string.Format("You Get entered: {0}","1" );
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

        public string LeavingMessage(string Name, string Tel, string Email, string OtherWay, string Message)
        {
            //WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            //Console.WriteLine(Name+Tel+Email+OtherWay+Message);
            return "LeavingMessage Success";
        }


        public string SendEmail(string Name, string Tel, string Email, string OtherWay, string Message)
        {
            return WcfService1.Email.Sendmail( Name,  Tel,  Email,  OtherWay,  Message);
        }

    }
    public class Email
    {
        public static string Sendmail(string Name, string Tel, string Email, string OtherWay, string Message)
        {
            //Console.WriteLine("用户名");
            string uid = "1510595785@qq.com";//发件人邮箱地址@符号前面的字符tom@dddd.com,则为"tom"  
            //Console.WriteLine("密码");//发件人邮箱的密码  
            string pwd = "fpzmnaizbbckgeff";//qwertyuiop...
            string ReceiveAddress = "209045565@qq.com";
            for (int i = 0; i < 1; i++) //连发20封，嘿嘿  
            {
                MailAddress from = new MailAddress(uid);
                MailAddress to = new MailAddress(ReceiveAddress);
                MailMessage mailMessage = new MailMessage(from, to);
                mailMessage.Subject = Name+" 给您留言";//邮件主题  
                mailMessage.Body = "电话:"+Tel+"\n"+"邮箱:"+Email+"\n"+"其他联系方式:"+OtherWay+"\n"+"留言内容:"+Message;//邮件正文  

                //添加附件  

                //string file1 = "test.txt";
                //Attachment attachment1 = new Attachment(file1, MediaTypeNames.Text.Plain);
                ////为附件添加时间信息  
                //ContentDisposition disposition1 = attachment1.ContentDisposition;
                //disposition1.CreationDate = File.GetCreationTime(file1);
                //disposition1.ModificationDate = File.GetLastWriteTime(file1);
                //disposition1.ReadDate = File.GetLastAccessTime(file1);
                //mailMessage.Attachments.Add(attachment1);

                //string file2 = "test.doc";
                //Attachment attachment2 = new Attachment(file2);
                ////为附件添加时间信息  
                //ContentDisposition disposition2 = attachment2.ContentDisposition;
                //disposition2.CreationDate = File.GetCreationTime(file2);
                //disposition2.ModificationDate = File.GetLastWriteTime(file2);
                //disposition2.ReadDate = File.GetLastAccessTime(file2);
                //mailMessage.Attachments.Add(attachment2);

                //实例化SmtpClient  
                SmtpClient smtpClient = new SmtpClient("smtp.qq.com");
                //设置验证发件人身份的凭据  
                smtpClient.Credentials = new NetworkCredential(uid, pwd);
                //发送  
                smtpClient.Send(mailMessage);

                //Console.WriteLine("OK - [{0}]", i);
                
            }
            return "OK";
            //Console.ReadKey();
        }
    }
}
