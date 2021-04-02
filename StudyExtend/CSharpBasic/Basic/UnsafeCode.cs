using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpBasic.Basic
{
    class UnsafeCode
    {
        public unsafe void Unsafe1()
        {
            int var = 20;
            int* p = &var;
            Console.WriteLine("Data is: {0} ", var);
            Console.WriteLine("p is: {0}",p->ToString());
            Console.WriteLine("Address is: {0}", (int)p);
        }
        public unsafe void Unsafe2()
        {
            int[] list = { 10, 100, 200 };
            fixed (int* ptr = list)

                /* 显示指针中数组地址 */
                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine("Address of list[{0}]={1}", i, (int)(ptr + i));
                    Console.WriteLine("Value of list[{0}]={1}", i, *(ptr + i));
                }
            
        }
    }
}
