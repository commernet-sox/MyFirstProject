using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace CPC
{
    /// <summary>
    /// mock request
    /// </summary>
    public sealed class HttpHelper
    {
        #region Transport Options
        /// <summary>
        /// http transport options
        /// </summary>
        public class HttpItem
        {
            /// <summary>
            /// url
            /// </summary>
            public string Url { get; set; }

            /// <summary>
            /// method
            /// </summary>
            public string Method { get; set; } = "GET";

            /// <summary>
            /// request timeout（ms）
            /// </summary>
            public int Timeout { get; set; } = 15000;

            /// <summary>
            /// read and write timeout（ms）
            /// </summary>
            public int ReadWriteTimeout { get; set; } = 30000;

            /// <summary>
            /// http headers
            /// </summary>
            public WebHeaderCollection Headers { get; set; }

            /// <summary>
            /// http accept value
            /// </summary>
            public string Accept { get; set; } = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";

            /// <summary>
            /// http content type
            /// </summary>
            public string ContentType { get; set; } = "application/x-www-form-urlencoded";

            /// <summary>
            /// http user agent
            /// </summary>
            public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:17.0) Gecko/20100101 Firefox/17.0";

            /// <summary>
            /// http referer url
            /// </summary>
            public string Referer { get; set; }

            /// <summary>
            /// add Ajax header
            /// </summary>
            public bool IsAjax { get; set; }

            /// <summary>
            /// http cookie header
            /// </summary>
            public string Cookie { get; set; }

            /// <summary>
            /// redirect response
            /// </summary>
            public bool AllowAutoRedirect { get; set; } = true;

            /// <summary>
            /// request encoding
            /// </summary>
            public Encoding Encoding { get; set; }

            /// <summary>
            /// maximum connection
            /// </summary>
            public int ConnectionLimit { get; set; }

            /// <summary>
            /// use 100-continue behavior
            /// </summary>
            public bool Expect100Continue { get; set; }

            /// <summary>
            /// request the associated cookies
            /// </summary>
            public CookieContainer CookieContainer { get; set; }

            /// <summary>
            /// cert file path
            /// </summary>
            public string CertPath { get; set; }

            /// <summary>
            /// vpn ip address
            /// </summary>
            public string ProxyIp { get; set; }

            /// <summary>
            /// vpn port
            /// </summary>
            public int ProxyPort { get; set; }

            /// <summary>
            /// vpn user name
            /// </summary>
            public string ProxyUserName { get; set; }

            /// <summary>
            /// vpn user password
            /// </summary>
            public string ProxyPwd { get; set; }

            /// <summary>
            /// post encoding 
            /// </summary>
            public Encoding PostEncoding { get; set; }

            /// <summary>
            /// post data
            /// </summary>
            public string PostData { get; set; }

            /// <summary>
            /// post stream
            /// </summary>
            public byte[] PostBytes { get; set; }

            /// <summary>
            /// post file path.
            /// </summary>
            public string PostFilePath { get; set; }
        }
        #endregion

        #region Http Result
        /// <summary>
        /// http result
        /// </summary>
        public class HttpResult
        {
            /// <summary>
            /// cookies array
            /// </summary>
            public CookieCollection Cookies { get; internal set; }

            /// <summary>
            /// cooike value
            /// </summary>
            public string Cookie { get; internal set; }

            /// <summary>
            /// html result
            /// </summary>
            public string Html { get; internal set; }

            /// <summary>
            /// stream
            /// </summary>
            public byte[] Bytes { get; internal set; }

            /// <summary>
            /// headers
            /// </summary>
            public WebHeaderCollection Headers { get; internal set; }

            /// <summary>
            /// status code 
            /// </summary>
            public HttpStatusCode StatusCode { get; internal set; }

            /// <summary>
            /// url
            /// </summary>
            public string ResponseUrl { get; internal set; }

            /// <summary>
            /// redirect url
            /// </summary>
            public string RedirectUrl
            {
                get
                {
                    if (Headers != null && Headers.Count > 0)
                    {
                        if (!string.IsNullOrWhiteSpace(Headers["location"]))
                        {
                            return Headers["location"].ToString();
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }

            /// <summary>
            /// status description
            /// </summary>
            public string StatusDescription { get; internal set; }
        }
        #endregion

        #region Main Methods
        #region Private
        /// <summary>
        /// get request result
        /// </summary>
        /// <param name="item">option</param>
        /// <param name="needHtml">need to return the html text</param>
        /// <returns>result</returns>
        private static HttpResult GetRequestData(HttpItem item, bool needHtml = true)
        {
            var result = new HttpResult();
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(item.Url);
                SetRequestAttr(item, request);
                var response = (HttpWebResponse)request.GetResponse();
                result.StatusCode = response.StatusCode;
                result.StatusDescription = response.StatusDescription;
                result.Headers = response.Headers;
                result.Cookies = response.Cookies;
                if (response.ResponseUri != null)
                {
                    result.ResponseUrl = response.ResponseUri.ToString();
                }

                if (!string.IsNullOrWhiteSpace(response.Headers["set-cookie"]))
                {
                    result.Cookie = GetCookie(response.Headers["set-cookie"]);
                }

                ProcessResponse(response, item, result, needHtml);
                return result;
            }
            catch (WebException ex)
            {
                result.Html = ex.Message;
                var response = (HttpWebResponse)ex.Response;

                if (response != null)
                {
                    result.StatusCode = response.StatusCode;
                    result.StatusDescription = result.StatusDescription;
                    ProcessResponse(response, item, result, needHtml);
                }
                else
                {
                    result.StatusCode = HttpStatusCode.NotFound;
                    result.StatusDescription = ex.Message;
                }

                return result;
            }
        }

        /// <summary>
        /// set properties for Request
        /// </summary>
        /// <param name="item">option</param>
        /// <param name="request"></param>
        private static void SetRequestAttr(HttpItem item, HttpWebRequest request)
        {
            SetCert(item, request);

            //set headers
            if (item.Headers != null && item.Headers.Count > 0)
            {
                request.Headers = item.Headers;
            }

            if (item.IsAjax)
            {
                request.Headers.Add("x-requested-with: XMLHttpRequest");
            }

            SetProxy(item, request);

            request.Method = item.Method;
            request.Timeout = item.Timeout;
            request.ReadWriteTimeout = item.ReadWriteTimeout;
            request.Accept = item.Accept;
            request.ContentType = item.ContentType;
            request.UserAgent = item.UserAgent;
            SetCooikes(item, request);
            request.Referer = item.Referer;
            request.AllowAutoRedirect = item.AllowAutoRedirect;
            if (item.ConnectionLimit > 0)
            {
                request.ServicePoint.ConnectionLimit = item.ConnectionLimit;
            }

            request.ServicePoint.Expect100Continue = item.Expect100Continue;
            SetPostData(item, request);
        }

        /// <summary>
        /// set cert
        /// </summary>
        /// <param name="item">option</param>
        /// <param name="request"></param>
        private static void SetCert(HttpItem item, HttpWebRequest request)
        {
            if (!string.IsNullOrWhiteSpace(item.CertPath))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                var cert = new X509Certificate(item.CertPath);
                request.ClientCertificates.Add(cert);
            }
        }

        /// <summary>
        /// cert callback result
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) => true;

        /// <summary>
        /// set vpn
        /// </summary>
        /// <param name="item">option</param>
        /// <param name="request"></param>
        private static void SetProxy(HttpItem item, HttpWebRequest request)
        {
            if (!string.IsNullOrWhiteSpace(item.ProxyIp))
            {
                WebProxy proxy;
                if (item.ProxyPort > 0)
                {
                    proxy = new WebProxy(item.ProxyIp, item.ProxyPort);
                }
                else
                {
                    proxy = new WebProxy(item.ProxyIp, false);
                }

                proxy.Credentials = new NetworkCredential(item.ProxyUserName, item.ProxyPwd);
                request.Proxy = proxy;
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
        }

        /// <summary>
        /// set cookies
        /// </summary>
        /// <param name="item">option</param>
        /// <param name="request"></param>
        private static void SetCooikes(HttpItem item, HttpWebRequest request)
        {
            if (!string.IsNullOrWhiteSpace(item.Cookie))
            {
                request.Headers[HttpRequestHeader.Cookie] = item.Cookie;
            }

            if (item.CookieContainer != null)
            {
                request.CookieContainer = item.CookieContainer;
            }
        }

        /// <summary>
        /// set post data
        /// </summary>
        /// <param name="item">option</param>
        /// <param name="request"></param>
        private static void SetPostData(HttpItem item, HttpWebRequest request)
        {
            if (item.Method.ToUpper() == "POST")
            {
                if (item.PostBytes != null && item.PostBytes.Length > 0)
                {
                    request.ContentLength = item.PostBytes.Length;
                    request.GetRequestStream().Write(item.PostBytes, 0, item.PostBytes.Length);
                }
                else if (!string.IsNullOrWhiteSpace(item.PostFilePath))
                {
                    using (var fs = new FileStream(item.PostFilePath, FileMode.Open, FileAccess.Read))
                    {
                        var bin = new BinaryReader(fs);
                        var bytes = bin.ReadBytes((int)fs.Length);
                        request.ContentLength = bytes.Length;
                        request.GetRequestStream().Write(bytes, 0, bytes.Length);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(item.PostData))
                {
                    var bytes = item.PostEncoding.GetBytes(item.PostData);
                    request.ContentLength = bytes.Length;
                    request.GetRequestStream().Write(bytes, 0, bytes.Length);
                }
            }
        }

        /// <summary>
        /// unified processing of returned http results
        /// </summary>
        /// <param name="response">web response</param>
        /// <param name="item">option</param>
        /// <param name="result">result</param>
        /// <param name="needHtml">need to return the html text</param>
        private static void ProcessResponse(HttpWebResponse response, HttpItem item, HttpResult result, bool needHtml)
        {
            MemoryStream stream;
            if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
            {
                stream = GetMemoryStream(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress));
            }
            else
            {
                stream = GetMemoryStream(response.GetResponseStream());
            }

            var array = stream.ToArray();
            result.Bytes = array;

            if (!needHtml)
            {
                return;
            }

            //auto get encoding
            if (item.Encoding == null)
            {
                var str = Encoding.Default.GetString(array, 0, array.Length);
                var match = Regex.Match(str, "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                var text = (match.Groups.Count > 2) ? match.Groups[2].Value : string.Empty;
                if (text.Contains("\""))
                {
                    text = text.Split('"')[0];
                }

                if (text.Contains(" "))
                {
                    text = text.Split(' ')[0];
                }

                text = text.Replace("\"", string.Empty).Replace("'", string.Empty).Replace(";", string.Empty);
                if (text.Length > 0)
                {
                    text = text.ToLower().Replace("iso-8859-1", "gbk");
                    if (string.IsNullOrWhiteSpace(response.CharacterSet.Trim()) || response.CharacterSet.Trim().Contains("utf8"))
                    {
                        item.Encoding = Encoding.UTF8;
                    }
                    else if (text != response.CharacterSet)
                    {
                        item.Encoding = Encoding.GetEncoding(text);
                    }
                    else
                    {
                        item.Encoding = Encoding.GetEncoding(response.CharacterSet);
                    }
                }
                else if (response.CharacterSet != null)
                {
                    if (response.CharacterSet.ToLower().Trim() == "iso-8859-1")
                    {
                        item.Encoding = Encoding.UTF8;
                    }
                    else if (string.IsNullOrWhiteSpace(response.CharacterSet.Trim()) || response.CharacterSet.Trim().Contains("utf8"))
                    {
                        item.Encoding = Encoding.UTF8;
                    }
                    else
                    {
                        item.Encoding = Encoding.GetEncoding(response.CharacterSet);
                    }
                }
                else
                {
                    item.Encoding = Encoding.UTF8;
                }
            }

            try
            {
                if (array.Length > 0)
                {
                    result.Html = item.Encoding.GetString(array);
                }
                else
                {
                    result.Html = string.Empty;
                }

                stream.Close();
                response.Close();
            }
            catch
            {
                result.Html = string.Empty;
            }
        }

        /// <summary>
        /// process memory stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static MemoryStream GetMemoryStream(Stream stream)
        {
            var mstream = new MemoryStream();
            var num = 256;
            var buffer = new byte[num];
            for (var i = stream.Read(buffer, 0, num); i > 0; i = stream.Read(buffer, 0, num))
            {
                mstream.Write(buffer, 0, i);
            }

            return mstream;
        }

        /// <summary>
        /// process cookies
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        private static string GetCookie(string cookies)
        {
            try
            {
                var text = string.Empty;
                cookies = cookies.Replace(";", "; ");
                var regex = new Regex("(?<=,)(?<cookie>[^ ]+=(?!deleted;)[^;]+);");
                var match = regex.Match("," + cookies);
                while (match.Success)
                {
                    text = text + match.Groups["cookie"].Value + ";";
                    match = match.NextMatch();
                }

                return text;
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        #region Public
        /// <summary>
        /// get request result
        /// </summary>
        /// <param name="item">option</param>
        /// <param name="needHtml">need to return the html text</param>
        /// <returns></returns>
        public static HttpResult GetResult(HttpItem item, bool needHtml = true)
        {
            var result = GetRequestData(item, needHtml);
            return result;
        }

        /// <summary>
        /// get request result from url
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="needHtml">need to return the html text</param>
        /// <returns></returns>
        public static HttpResult GetResult(string url, bool needHtml = true)
        {
            var item = new HttpItem() { Url = url };
            return GetResult(item, needHtml);
        }

        /// <summary>
        /// get request html
        /// </summary>
        /// <param name="item">option</param>
        /// <returns></returns>
        public static string GetHtml(HttpItem item)
        {
            var result = GetRequestData(item);
            return result.Html;
        }

        /// <summary>
        /// get request html from url
        /// </summary>
        /// <param name="url">url</param>
        /// <returns></returns>
        public static string GetHtml(string url)
        {
            var item = new HttpItem() { Url = url };
            return GetHtml(item);
        }

        /// <summary>
        /// get request stream
        /// </summary>
        /// <param name="item">option</param>
        /// <returns></returns>
        public static byte[] GetBytes(HttpItem item)
        {
            var result = GetRequestData(item, false);
            return result.Bytes;
        }

        /// <summary>
        /// get request stream from url
        /// </summary>
        /// <param name="url">url</param>
        /// <returns></returns>
        public static byte[] GetBytes(string url)
        {
            var item = new HttpItem() { Url = url };
            return GetBytes(item);
        }
        #endregion
        #endregion
    }
}
