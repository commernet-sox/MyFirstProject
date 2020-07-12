using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices.WindowsRuntime;
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
