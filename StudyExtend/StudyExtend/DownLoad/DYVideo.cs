using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace StudyExtend.DownLoad
{
    public class DYVideo
    {
        public static void DownLoad_Video()
        {
            try
            {
                //Console.WriteLine("请输入要下载内容的链接,输入exit退出程序:");
                var url = Console.ReadLine();
                //string pathUrl = "";
                //if (string.IsNullOrWhiteSpace(url))
                //{
                //    Console.WriteLine("内容不能为空!");
                //}
                //else
                //{
                //    if (url == "exit")
                //    {
                //        return;
                //    }
                //    pathUrl = url;
                //}
                string pathUrl = "https://www.douyin.com/aweme/v1/play/?video_id=v0300fg10000c3c1b0i14qtjcm1qt9dg&line=0&file_id=4afbeb5d957e4c0a83999559fae05558&sign=93de4aa0a46fec8b35c9ab4e08072280&is_play_url=1&source=PackSourceEnum_FEED";
                System.Net.HttpWebRequest request = null;
                System.Net.HttpWebResponse response = null;
                //请求网络路径地址
                
                request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(pathUrl);
                //System.Net.WebProxy proxy = new WebProxy("39.105.128.50", 8080);//指定的ip和端口
                //request.Proxy = proxy;
                request.Timeout = 30000; // 超时时间
                //request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");//主要用来将我们的爬虫伪装成浏览器。
                //request.Headers.Add("cookie", "ttwid=1%7CjfQZLHvQdK0FTh6IL2GZC43HUhoTOeSuvvXTSKKDLH4%7C1626140582%7C8ef0ca157f402e932da7cbd51cbd748f0397727d443214378b3a012f4604b439; MONITOR_WEB_ID=aac6ca05-7709-4dd3-b926-98e9fb5b0e4a; passport_csrf_token_default=7ddb2d1e32ce394e58f3a0837e53735d; passport_csrf_token=7ddb2d1e32ce394e58f3a0837e53735d; douyin.com; s_v_web_id=verify_kr49itfj_T3i1g5iC_JXhm_4rSL_BBOT_eOKtgkIx3fLw");
                //request.Headers.Add("referer", "https://www.douyin.com/");
                //request.Headers.Add("accept", "*/*");
                //request.Headers.Add("withcredentials", "true");
                //request.AllowAutoRedirect = true;
                //request.CookieContainer=new System.Net.CookieContainer();//主要用来保存爬虫的登录状态。
                //获得请求结果
                response = (System.Net.HttpWebResponse)request.GetResponse();
                long totalBytes = response.ContentLength; //从WEB响应得到总字节数
                //更新进度
                Console.WriteLine("进度:0%");
                long totalDownloadedByte = 0; //下载文件大小   
                //文件下载地址
                string path = System.IO.Directory.GetCurrentDirectory() + @"\temp";

                // 如果不存在就创建file文件夹
                if (!Directory.Exists(path))
                {
                    if (path != null) Directory.CreateDirectory(path);
                }
                path = path + $"/NO{(DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000}.mp4";
                Stream stream = response.GetResponseStream();
                //先创建文件
                Stream sos = new System.IO.FileStream(path, System.IO.FileMode.Create);
                byte[] img = new byte[1024];
                int total = stream.Read(img, 0, img.Length);
                while (total > 0)
                {
                    totalDownloadedByte = total + totalDownloadedByte; //更新文件大小
                    Console.WriteLine($"进度:{ totalDownloadedByte / totalBytes * 100}%");                                  
                    //之后再输出内容
                    sos.Write(img, 0, total);
                    total = stream.Read(img, 0, img.Length);
                }
                Console.WriteLine("进度:100%");
                stream.Close();
                stream.Dispose();
                sos.Close();
                sos.Dispose();
                Console.WriteLine($"下载完成.路径{path}");
                //DownLoad_Video();
            }
            catch (Exception ex)
            {
                Console.WriteLine("下载失败："+ex.Message);
                DownLoad_Video();
            }
            
        }

        /// 下载带进度条代码（普通进度条）  
        /// </summary>  
        /// <param name="URL">网址</param>  
        /// <param name="Filename">下载后文件名为</param>  
        /// <param name="Prog">报告进度的处理(第一个参数：总大小，第二个参数：当前进度)</param>  
        /// <returns>True/False是否下载成功</returns>  
        //public static bool DownLoadFile(string URL, string Filename, Action<int, int> updateProgress = null)
        //{
        //    Stream st = null;
        //    Stream so = null;
        //    System.Net.HttpWebRequest Myrq = null;
        //    System.Net.HttpWebResponse myrp = null;
        //    bool flag = false;
        //    try
        //    {
        //        Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(URL); //从URL地址得到一个WEB请求     
        //        myrp = (System.Net.HttpWebResponse)Myrq.GetResponse(); //从WEB请求得到WEB响应     
        //        long totalBytes = myrp.ContentLength; //从WEB响应得到总字节数
        //        //更新进度
        //        if (updateProgress != null)
        //        {
        //            updateProgress((int)totalBytes, 0);//从总字节数得到进度条的最大值  
        //        }
        //        st = myrp.GetResponseStream(); //从WEB请求创建流（读）     
        //        so = new System.IO.FileStream(Filename, System.IO.FileMode.Create); //创建文件流（写）     
        //        long totalDownloadedByte = 0; //下载文件大小     
        //        byte[] by = new byte[1024];
        //        int osize = st.Read(by, 0, (int)by.Length); //读流     
        //        while (osize > 0)
        //        {
        //            totalDownloadedByte = osize + totalDownloadedByte; //更新文件大小     
        //            Application.DoEvents();
        //            so.Write(by, 0, osize); //写流     
        //            //更新进度
        //            if (updateProgress != null)
        //            {
        //                updateProgress((int)totalBytes, (int)totalDownloadedByte);//更新进度条 
        //            }
        //            osize = st.Read(by, 0, (int)by.Length); //读流     
        //        }
        //        //更新进度
        //        if (updateProgress != null)
        //        {
        //            updateProgress((int)totalBytes, (int)totalBytes);
        //        }
        //        flag = true;
        //    }
        //    catch (Exception)
        //    {
        //        flag = false;
        //        throw;
        //        //return false;
        //    }
        //    finally
        //    {
        //        if (Myrq != null)
        //        {
        //            Myrq.Abort();//销毁关闭连接
        //        }
        //        if (myrp != null)
        //        {
        //            myrp.Close();//销毁关闭响应
        //        }
        //        if (so != null)
        //        {
        //            so.Close(); //关闭流 
        //        }
        //        if (st != null)
        //        {
        //            st.Close(); //关闭流  
        //        }
        //    }
        //    return flag;
        //}
    }
}
