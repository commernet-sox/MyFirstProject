using Sodao.Snap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScreenShot1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("111...");
            // 根据网址生成快照
            SDWebCache wc = new SDWebCache("http://localhost:23099/bi/OperationReport/WebView?WarehouseId=HD-DZ&ORHDate=2020-08-01&EndORHDate=2020-08-03&Status=10");
            Bitmap image = wc.Snap();
            image.Save("render_img1.bmp");
        }
    }
}
