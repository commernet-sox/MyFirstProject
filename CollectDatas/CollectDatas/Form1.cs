using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CollectDatas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("程序已开始...");
            //请求的URL链接
            String URL = "https://www.zgzckpw.com/personal.php?";
            //创建HttpWebRequest 
            HttpWebRequest myrq = (HttpWebRequest)WebRequest.Create(URL);

            myrq.KeepAlive = false;
            myrq.Timeout = 30 * 1000; //超时时间
            myrq.Method = "Get";  //请求方式 
            myrq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            myrq.Host = "www.zgzckpw.com"; //来源
                                           //定义请求请求Referer 
            myrq.Referer = "https://www.zgzckpw.com/personal.php?";
            
            //定义浏览器代理
            myrq.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 UBrowser/6.2.4098.3 Safari/537.36";

            //请求网页
            HttpWebResponse myrp = (HttpWebResponse)myrq.GetResponse();

            //判断请求状态
            if (myrp.StatusCode != HttpStatusCode.OK)
            {
                return;
            }

            //打印网页源码
            using (StreamReader sr = new StreamReader(myrp.GetResponseStream()))
            {
                Console.WriteLine(sr.ReadToEnd());
            }

        }
    }
}
