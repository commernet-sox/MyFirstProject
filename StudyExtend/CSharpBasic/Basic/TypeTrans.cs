using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpBasic.Basic
{
    /// <summary>
    /// 类型转换
    /// </summary>
    public class TypeTrans
    {
        public string str = "s";
        public int? num = null;
        public object obj = 256;
        public int numb = 30;
        public void Trans()
        {
            Console.WriteLine(Convert.ToBoolean(num));
            Console.WriteLine(Convert.ToByte(num));//把对象转为0-255数字
            Console.WriteLine(Convert.ToChar(str));
            Console.WriteLine(sizeof(Int32));
            Console.WriteLine(~numb);
            //获取堆上变量的地址
            unsafe
            {
                fixed (char* p = str)
                {
                    Console.WriteLine("Address of str 0x{0:x}", (int)p);
                }
            }
            //获取栈上变量的地址
            int number=1;
            unsafe
            {
                int* p = &number;
                *p = 0xffff;
                Console.WriteLine("{0:x}", *p);
                Console.WriteLine("Address of number:0x{0:x}", (int)p);
            }
        }
    }
}
