using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CPC
{
    /// <summary>
    /// network related operations tool class
    /// </summary>
    public sealed class NetworkUtility
    {
        #region Email
        /// <summary>
        /// send email
        /// </summary>
        /// <param name="option">email related options</param>
        public static void SendMail(EmailOption option)
        {
            var from = new MailAddress(option.UserName, option.DisplayName);
            using (var msg = new MailMessage())
            {
                if (option.Adjuncts != null)
                {
                    foreach (var item in option.Adjuncts)
                    {
                        msg.Attachments.Add(new Attachment(item));
                    }
                }

                if (option.CC != null)
                {
                    foreach (var item in option.CC)
                    {
                        msg.CC.Add(item);
                    }
                }

                if (option.To != null)
                {
                    foreach (var item in option.To)
                    {
                        msg.To.Add(item);
                    }
                }

                msg.Subject = option.Subject;
                msg.SubjectEncoding = option.SubjectEncode;
                msg.Body = option.Body;
                msg.BodyEncoding = option.BodyEncode;
                msg.IsBodyHtml = option.IsBodyHtml;
                msg.Priority = option.Priority;
                msg.From = from;

                using (var client = new SmtpClient
                {
                    Host = option.Host,
                    Credentials = new NetworkCredential(option.UserName, option.Password),
                    DeliveryMethod = option.DeliveryMethod,
                    DeliveryFormat = option.DeliveryFormat,
                    EnableSsl = option.EnableSsl
                })
                {

                    if (option.Port > 0)
                    {
                        client.Port = option.Port;
                    }

                    client.Send(msg);
                }
            }
        }
        #endregion

        #region Download
        /// <summary>
        /// download file
        /// </summary>
        /// <param name="url">file url</param>
        /// <param name="savePath">save file path</param>
        public static void DownloadFile(string url, string savePath)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(url, savePath);
            }
        }
        #endregion
    }

    public class EmailOption
    {
        /// <summary>
        /// smtp server host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// smtp server port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// smtp client whether to use SSL
        /// </summary>
        public bool EnableSsl { get; set; } = false;

        /// <summary>
        /// sender dispaly name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// sender email address
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// sender email password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The delivery format used by smtp client
        /// </summary>
        public SmtpDeliveryFormat DeliveryFormat { get; set; } = SmtpDeliveryFormat.International;

        /// <summary>
        /// email delivery mode
        /// </summary>
        public SmtpDeliveryMethod DeliveryMethod { get; set; } = SmtpDeliveryMethod.Network;

        /// <summary>
        /// email message title
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// email message title encoding
        /// </summary>
        public Encoding SubjectEncode { get; set; } = Encoding.UTF8;

        /// <summary>
        /// email message main content
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// email message body whether support html format
        /// </summary>
        public bool IsBodyHtml { get; set; } = true;

        /// <summary>
        /// email message body encoding
        /// </summary>
        public Encoding BodyEncode { get; set; } = Encoding.UTF8;

        /// <summary>
        /// email priority
        /// </summary>
        public MailPriority Priority { get; set; } = MailPriority.Normal;

        /// <summary>
        /// email message adjuncts path (local file path)
        /// </summary>
        public IEnumerable<string> Adjuncts { get; set; }

        /// <summary>
        /// email cc list
        /// </summary>
        public IEnumerable<string> CC { get; set; }

        public IEnumerable<string> To { get; set; }
    }
}
