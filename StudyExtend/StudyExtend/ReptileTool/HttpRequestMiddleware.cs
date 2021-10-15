using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Security;
using System.Web;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Threading.Tasks;

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

        private static string Signature(string access_token, string plainText)
        {
            using (HMACSHA1 mac = new HMACSHA1(Encoding.UTF8.GetBytes(access_token)))
            {
                var hash = mac.ComputeHash(Encoding.UTF8.GetBytes(plainText));
                var pText = Encoding.UTF8.GetBytes(plainText);
                var all = new byte[hash.Length + pText.Length];
                Array.Copy(hash, 0, all, 0, hash.Length);
                Array.Copy(pText, 0, all, hash.Length, pText.Length);
                return Convert.ToBase64String(all);
            }
        }

        public static string SendRequest(string url, string accesstoken, Dictionary<string, string> data, HttpMethod requestMethod,
           Dictionary<string, string> header, int timeOut, List<UploadFileInfo> listlocalPath = null, int offset = -1, int sliceSize = 0)
        {
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                var paramStr = "";
                if (requestMethod == HttpMethod.Get || requestMethod == HttpMethod.Delete)
                {
                    foreach (var key in data.Keys)
                    {
                        paramStr += string.Format("{0}={1}&", key, HttpUtility.UrlEncode(data[key].ToString()));
                    }
                    paramStr = paramStr.TrimEnd('&');
                    url += (url.EndsWith("?") ? "&" : "?") + paramStr;
                    header.Add("sign", Signature(accesstoken, paramStr));
                }


                var request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Accept = "*/*";
                request.KeepAlive = true;
                request.Method = requestMethod.ToString().ToUpper();

                request.UserAgent = "ruyicang-netcore-sdk";
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
                if (requestMethod == HttpMethod.Post)
                {
                    var memStream = new MemoryStream();
                    if (header.ContainsKey("Content-Type") && header["Content-Type"] == "application/json")
                    {
                        var jsonsetting = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Include
                        };
                        var json = JsonConvert.SerializeObject(data, jsonsetting);
                        var jsonByte = Encoding.GetEncoding("utf-8").GetBytes(json.ToString());
                        memStream.Write(jsonByte, 0, jsonByte.Length);
                    }
                    else
                    {
                        var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
                        var beginBoundary = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                        var endBoundary = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                        request.ContentType = "multipart/form-data; boundary=" + boundary;

                        var strBuf = new StringBuilder();
                        paramStr = string.Empty;
                        foreach (var key in data.Keys)
                        {
                            strBuf.Append("\r\n--" + boundary + "\r\n");
                            strBuf.Append("Content-Disposition: form-data; name=\"" + key + "\"\r\n\r\n");
                            strBuf.Append(data[key].ToString());
                            paramStr += string.Format("{0}={1}&", key, data[key].ToString());
                        }
                        paramStr = paramStr.TrimEnd('&');


                        if (request.Headers["sign"] == null)
                            request.Headers.Add("sign", Signature(accesstoken, paramStr));
                        else
                            request.Headers["sign"] = Signature(accesstoken, paramStr);


                        var paramsByte = Encoding.GetEncoding("utf-8").GetBytes(strBuf.ToString());
                        memStream.Write(paramsByte, 0, paramsByte.Length);

                        if (listlocalPath != null)
                        {
                            foreach (UploadFileInfo fi in listlocalPath)
                            {
                                memStream.Write(beginBoundary, 0, beginBoundary.Length);
                                if (!string.IsNullOrEmpty(fi.localPath))
                                {
                                    var fileInfo = new FileInfo(fi.localPath);

                                    var fileStream = new FileStream(fi.localPath, FileMode.Open, FileAccess.Read);

                                    string filePartHeader =
                                        "Content-Disposition: form-data; name=\"" + fi.Name + "\"; filename=\"{0}\"\r\n" +
                                        "Content-Type: application/octet-stream\r\n\r\n";
                                    var headerText = string.Format(filePartHeader, fileInfo.Name);
                                    var headerbytes = Encoding.UTF8.GetBytes(headerText);
                                    memStream.Write(headerbytes, 0, headerbytes.Length);

                                    if (offset == -1)
                                    {
                                        var buffer = new byte[1024];
                                        int bytesRead;
                                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                                        {
                                            memStream.Write(buffer, 0, bytesRead);
                                        }
                                    }
                                    else
                                    {
                                        var buffer = new byte[sliceSize];
                                        int bytesRead;
                                        fileStream.Seek(offset, SeekOrigin.Begin);
                                        bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                                        memStream.Write(buffer, 0, bytesRead);
                                    }
                                }
                                else
                                {
                                    string filePartHeader =
                                      "Content-Disposition: form-data; name=\"" + fi.Name + "\"; filename=\"{0}\"\r\n" +
                                      "Content-Type: application/octet-stream\r\n\r\n";
                                    var headerText = string.Format(filePartHeader, Guid.NewGuid().ToString());
                                    var headerbytes = Encoding.UTF8.GetBytes(headerText);
                                    memStream.Write(headerbytes, 0, headerbytes.Length);

                                    if (offset == -1)
                                    {
                                        var buffer = new byte[1024];
                                        int bytesRead;
                                        while ((bytesRead = fi.Stream.Read(buffer, 0, buffer.Length)) != 0)
                                        {
                                            memStream.Write(buffer, 0, bytesRead);
                                        }
                                    }
                                    else
                                    {
                                        var buffer = new byte[sliceSize];
                                        int bytesRead;
                                        fi.Stream.Seek(offset, SeekOrigin.Begin);
                                        bytesRead = fi.Stream.Read(buffer, 0, buffer.Length);
                                        memStream.Write(buffer, 0, bytesRead);
                                    }
                                }
                            }
                        }
                        memStream.Write(endBoundary, 0, endBoundary.Length);
                    }
                    request.ContentLength = memStream.Length;
                    var requestStream = request.GetRequestStream();
                    memStream.Position = 0;
                    var tempBuffer = new byte[memStream.Length];
                    memStream.Read(tempBuffer, 0, tempBuffer.Length);
                    memStream.Close();

                    requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                    requestStream.Close();
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

        public static string SendRequest(string url, string accesstoken, Dictionary<string, object> data, HttpMethod requestMethod,
   Dictionary<string, string> header, int timeOut, List<UploadFileInfo> listlocalPath = null, int offset = -1, int sliceSize = 0)
        {
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                var paramStr = "";
                if (requestMethod == HttpMethod.Get || requestMethod == HttpMethod.Delete)
                {
                    foreach (var key in data.Keys)
                    {
                        paramStr += string.Format("{0}={1}&", key, HttpUtility.UrlEncode(data[key].ToString()));
                    }
                    paramStr = paramStr.TrimEnd('&');
                    url += (url.EndsWith("?") ? "&" : "?") + paramStr;
                    header.Add("sign", Signature(accesstoken, paramStr));
                }


                var request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Accept = "*/*";
                request.KeepAlive = true;
                request.Method = requestMethod.ToString().ToUpper();

                request.UserAgent = "smartvisioon-net-sdk";
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
                if (requestMethod == HttpMethod.Post)
                {
                    var memStream = new MemoryStream();
                    if (header.ContainsKey("Content-Type") && header["Content-Type"] == "application/json")
                    {
                        var jsonsetting = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Include
                        };
                        var json = JsonConvert.SerializeObject(data, jsonsetting);
                        var jsonByte = Encoding.GetEncoding("utf-8").GetBytes(json.ToString());
                        memStream.Write(jsonByte, 0, jsonByte.Length);
                    }
                    else
                    {
                        var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
                        var beginBoundary = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                        var endBoundary = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                        request.ContentType = "multipart/form-data; boundary=" + boundary;

                        var strBuf = new StringBuilder();
                        paramStr = string.Empty;
                        foreach (var key in data.Keys)
                        {
                            strBuf.Append("\r\n--" + boundary + "\r\n");
                            strBuf.Append("Content-Disposition: form-data; name=\"" + key + "\"\r\n\r\n");
                            strBuf.Append(data[key].ToString());
                            paramStr += string.Format("{0}={1}&", key, data[key].ToString());
                        }
                        paramStr = paramStr.TrimEnd('&');


                        if (request.Headers["sign"] == null)
                            request.Headers.Add("sign", Signature(accesstoken, paramStr));
                        else
                            request.Headers["sign"] = Signature(accesstoken, paramStr);


                        var paramsByte = Encoding.GetEncoding("utf-8").GetBytes(strBuf.ToString());
                        memStream.Write(paramsByte, 0, paramsByte.Length);

                        if (listlocalPath != null)
                        {
                            foreach (UploadFileInfo fi in listlocalPath)
                            {
                                memStream.Write(beginBoundary, 0, beginBoundary.Length);
                                if (!string.IsNullOrEmpty(fi.localPath))
                                {
                                    var fileInfo = new FileInfo(fi.localPath);

                                    var fileStream = new FileStream(fi.localPath, FileMode.Open, FileAccess.Read);

                                    string filePartHeader =
                                        "Content-Disposition: form-data; name=\"" + fi.Name + "\"; filename=\"{0}\"\r\n" +
                                        "Content-Type: application/octet-stream\r\n\r\n";
                                    var headerText = string.Format(filePartHeader, fileInfo.Name);
                                    var headerbytes = Encoding.UTF8.GetBytes(headerText);
                                    memStream.Write(headerbytes, 0, headerbytes.Length);

                                    if (offset == -1)
                                    {
                                        var buffer = new byte[1024];
                                        int bytesRead;
                                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                                        {
                                            memStream.Write(buffer, 0, bytesRead);
                                        }
                                    }
                                    else
                                    {
                                        var buffer = new byte[sliceSize];
                                        int bytesRead;
                                        fileStream.Seek(offset, SeekOrigin.Begin);
                                        bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                                        memStream.Write(buffer, 0, bytesRead);
                                    }
                                }
                                else
                                {
                                    string filePartHeader =
                                      "Content-Disposition: form-data; name=\"" + fi.Name + "\"; filename=\"{0}\"\r\n" +
                                      "Content-Type: application/octet-stream\r\n\r\n";
                                    var headerText = string.Format(filePartHeader, Guid.NewGuid().ToString());
                                    var headerbytes = Encoding.UTF8.GetBytes(headerText);
                                    memStream.Write(headerbytes, 0, headerbytes.Length);

                                    if (offset == -1)
                                    {
                                        var buffer = new byte[1024];
                                        int bytesRead;
                                        while ((bytesRead = fi.Stream.Read(buffer, 0, buffer.Length)) != 0)
                                        {
                                            memStream.Write(buffer, 0, bytesRead);
                                        }
                                    }
                                    else
                                    {
                                        var buffer = new byte[sliceSize];
                                        int bytesRead;
                                        fi.Stream.Seek(offset, SeekOrigin.Begin);
                                        bytesRead = fi.Stream.Read(buffer, 0, buffer.Length);
                                        memStream.Write(buffer, 0, bytesRead);
                                    }
                                }
                            }
                        }
                        memStream.Write(endBoundary, 0, endBoundary.Length);
                    }
                    request.ContentLength = memStream.Length;
                    var requestStream = request.GetRequestStream();
                    memStream.Position = 0;
                    var tempBuffer = new byte[memStream.Length];
                    memStream.Read(tempBuffer, 0, tempBuffer.Length);
                    memStream.Close();

                    requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                    requestStream.Close();
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

        public static async Task<string> SendRequestAsync(string url, string accesstoken, Dictionary<string, string> data, HttpMethod requestMethod,
   Dictionary<string, string> header, int timeOut, List<UploadFileInfo> listlocalPath = null, int offset = -1, int sliceSize = 0)
        {
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                var paramStr = "";
                if (requestMethod == HttpMethod.Get || requestMethod == HttpMethod.Delete)
                {
                    foreach (var key in data.Keys)
                    {
                        paramStr += string.Format("{0}={1}&", key, HttpUtility.UrlEncode(data[key].ToString()));
                    }
                    paramStr = paramStr.TrimEnd('&');
                    url += (url.EndsWith("?") ? "&" : "?") + paramStr;
                    header.Add("sign", Signature(accesstoken, paramStr));
                }


                var request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Accept = "*/*";
                request.KeepAlive = true;
                request.Method = requestMethod.ToString().ToUpper();
                request.UserAgent = "smartvisioon-net-sdk";
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
                if (requestMethod == HttpMethod.Post)
                {
                    var memStream = new MemoryStream();
                    if (header.ContainsKey("Content-Type") && header["Content-Type"] == "application/json")
                    {
                        var jsonsetting = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Include
                        };
                        var json = JsonConvert.SerializeObject(data, jsonsetting);
                        var jsonByte = Encoding.GetEncoding("utf-8").GetBytes(json.ToString());
                        memStream.Write(jsonByte, 0, jsonByte.Length);
                    }
                    else
                    {
                        var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
                        var beginBoundary = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                        var endBoundary = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                        request.ContentType = "multipart/form-data; boundary=" + boundary;

                        var strBuf = new StringBuilder();
                        paramStr = string.Empty;
                        foreach (var key in data.Keys)
                        {
                            strBuf.Append("\r\n--" + boundary + "\r\n");
                            strBuf.Append("Content-Disposition: form-data; name=\"" + key + "\"\r\n\r\n");
                            strBuf.Append(data[key].ToString());
                            paramStr += string.Format("{0}={1}&", key, data[key].ToString());
                        }
                        paramStr = paramStr.TrimEnd('&');


                        if (request.Headers["sign"] == null)
                            request.Headers.Add("sign", Signature(accesstoken, paramStr));
                        else
                            request.Headers["sign"] = Signature(accesstoken, paramStr);


                        var paramsByte = Encoding.GetEncoding("utf-8").GetBytes(strBuf.ToString());
                        memStream.Write(paramsByte, 0, paramsByte.Length);

                        if (listlocalPath != null)
                        {
                            foreach (UploadFileInfo fi in listlocalPath)
                            {
                                memStream.Write(beginBoundary, 0, beginBoundary.Length);
                                if (!string.IsNullOrEmpty(fi.localPath))
                                {
                                    var fileInfo = new FileInfo(fi.localPath);

                                    var fileStream = new FileStream(fi.localPath, FileMode.Open, FileAccess.Read);

                                    string filePartHeader =
                                        "Content-Disposition: form-data; name=\"" + fi.Name + "\"; filename=\"{0}\"\r\n" +
                                        "Content-Type: application/octet-stream\r\n\r\n";
                                    var headerText = string.Format(filePartHeader, fileInfo.Name);
                                    var headerbytes = Encoding.UTF8.GetBytes(headerText);
                                    memStream.Write(headerbytes, 0, headerbytes.Length);

                                    if (offset == -1)
                                    {
                                        var buffer = new byte[1024];
                                        int bytesRead;
                                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                                        {
                                            memStream.Write(buffer, 0, bytesRead);
                                        }
                                    }
                                    else
                                    {
                                        var buffer = new byte[sliceSize];
                                        int bytesRead;
                                        fileStream.Seek(offset, SeekOrigin.Begin);
                                        bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                                        memStream.Write(buffer, 0, bytesRead);
                                    }
                                }
                                else
                                {
                                    string filePartHeader =
                                      "Content-Disposition: form-data; name=\"" + fi.Name + "\"; filename=\"{0}\"\r\n" +
                                      "Content-Type: application/octet-stream\r\n\r\n";
                                    var headerText = string.Format(filePartHeader, Guid.NewGuid().ToString());
                                    var headerbytes = Encoding.UTF8.GetBytes(headerText);
                                    memStream.Write(headerbytes, 0, headerbytes.Length);

                                    if (offset == -1)
                                    {
                                        var buffer = new byte[1024];
                                        int bytesRead;
                                        while ((bytesRead = fi.Stream.Read(buffer, 0, buffer.Length)) != 0)
                                        {
                                            memStream.Write(buffer, 0, bytesRead);
                                        }
                                    }
                                    else
                                    {
                                        var buffer = new byte[sliceSize];
                                        int bytesRead;
                                        fi.Stream.Seek(offset, SeekOrigin.Begin);
                                        bytesRead = fi.Stream.Read(buffer, 0, buffer.Length);
                                        memStream.Write(buffer, 0, bytesRead);
                                    }
                                }
                            }
                        }
                        memStream.Write(endBoundary, 0, endBoundary.Length);
                    }
                    request.ContentLength = memStream.Length;
                    var requestStream = request.GetRequestStream();
                    memStream.Position = 0;
                    var tempBuffer = new byte[memStream.Length];
                    memStream.Read(tempBuffer, 0, tempBuffer.Length);
                    memStream.Close();

                    await requestStream.WriteAsync(tempBuffer, 0, tempBuffer.Length);
                    requestStream.Close();
                }
                var response = await request.GetResponseAsync();
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

    public class UploadFileInfo
    {
        public string localPath { get; set; }

        public string Name { get; set; }

        public Stream Stream { get; set; }
    }
}
