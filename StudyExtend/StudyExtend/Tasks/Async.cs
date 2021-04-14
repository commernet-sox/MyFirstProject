using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StudyExtend.Tasks
{
    public class Async
    {
        //同步读取文件内容
        public static string GetContent(string fileName)
        {
            FileStream fs = new FileStream(fileName,FileMode.Open);
            var bytes = new byte[fs.Length];
            //Read方法同步读取内容，阻塞线程
            int len = fs.Read(bytes,0,bytes.Length);
            string result = Encoding.UTF8.GetString(bytes);
            return result;
        }
        //异步读取文件内容，由于获取了结果，会阻塞主线程
        public async static Task<string> GetContentAsync(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            var bytes = new byte[fs.Length];
            //Read方法同步读取内容，阻塞线程
            int len =await fs.ReadAsync(bytes, 0, bytes.Length);
            string result = Encoding.UTF8.GetString(bytes);
            return result;
        }
    }
}
