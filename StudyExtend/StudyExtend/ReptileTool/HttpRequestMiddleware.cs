using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace StudyExtend.ReptileTool
{
    public enum HttpMethod { Get, Post, Delete };
    public class HttpRequestMiddleware
    {
        public static string SendRequest(string url, Dictionary<string, string> data, HttpMethod requestMethod,
            Dictionary<string, string> header, int timeOut)
        {
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                if (requestMethod == HttpMethod.Get || requestMethod == HttpMethod.Delete)
                {
                    var paramStr = "";
                    foreach (var key in data.Keys)
                    {
                        paramStr += string.Format("{0}={1}&", key, HttpUtility.UrlEncode(data[key].ToString()));
                    }
                    paramStr = paramStr.TrimEnd('&');
                    if (!string.IsNullOrEmpty(paramStr))
                        url += (url.EndsWith("?") ? "&" : "?") + paramStr;
                }
                else
                {
                    throw new NotSupportedException("post方法请使用另外一个SendRequest");
                }

                var request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.KeepAlive = true;
                request.Timeout = timeOut;
                request.Method = requestMethod.ToString().ToUpper();
                request.Accept = "*/*";
                request.KeepAlive = true;
                request.UserAgent = "FRS-sdknet";
                request.Timeout = timeOut;
                foreach (var key in header.Keys)
                {
                    if (key == "Content-Type")
                    {
                        request.ContentType = header[key];
                    }
                    else
                    {
                        request.Headers.Add(key, header[key]);
                    }
                }
                var response = request.GetResponse();


                using (var s = response.GetResponseStream())
                {
                    var reader = new StreamReader(s, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException we)
            {
                if (we.Status == WebExceptionStatus.ProtocolError)
                {
                    using (var s = we.Response.GetResponseStream())
                    {
                        var reader = new StreamReader(s, Encoding.UTF8);
                        return reader.ReadToEnd();
                    }
                }
                else
                {

                    throw we;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
