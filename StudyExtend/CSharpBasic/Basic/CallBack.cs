using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpBasic.Basic
{
    class CallBack
    {
        //声明回调函数结构
        public delegate void Add(int a,int b);
        //声明回调变量
        public Add add = null;
        //设置回调函数，给回调函数赋值
        public void SetAddCallBack(Add add)
        {
            this.add = add;
        }
        //调用回调函数
        public void CallAdd()
        {
            if (this.add != null)
            {
                add(1,99);
            }
        }
    }
    class DoCallBack
    {
        //定义回调函数
        public void add(int a, int b)
        {
            Console.WriteLine(a+b);
        }
    }
}
