using System;
using System.Collections.Generic;
using System.Text;

namespace StudyExtend.HtmlToImage
{
    /// <summary>
    /// html转img图片
    /// </summary>
    public class DemoImage
    {
        string StartTime = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");
        string excelPath1 = AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.AddDays(-1).ToString("yyyyMMdd") + @"\";
        public void Demo()
        {
            try
            {
                //string BaseUrl = "http://opr.ruyicang.com/OperationReport/MasterWebView2?ORHDate={0}";
                string BaseUrl = "http://www.baidu.com";
                BaseUrl = string.Format(BaseUrl, StartTime);
                HtmlToImage htmlToImage = new HtmlToImage();
                htmlToImage.SaveImage(BaseUrl, excelPath1, "Master1", 1366);
                Console.WriteLine("生成图片成功...");
            }
            catch (Exception ex)
            {
                Console.WriteLine("生成总部播报图片错误:" + ex.Message);
            }
        }
    }
}
