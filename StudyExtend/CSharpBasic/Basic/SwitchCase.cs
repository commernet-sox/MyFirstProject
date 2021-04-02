using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpBasic.Basic
{
    public class SwitchCase
    {
        public void index()
        {
            /* 局部变量定义 */
            char grade = 'B';

            switch (grade)
            {
                case 'A':
                    Console.WriteLine("很棒！");
                    break;
                case 'B':
                case 'C':
                    Console.WriteLine("做得好");
                    goto case 'D';
                case 'D':
                    Console.WriteLine("您通过了");
                    break;
                case 'F':
                    Console.WriteLine("最好再试一下");
                    break;
                default:
                    Console.WriteLine("无效的成绩");
                    break;
            }
            Console.WriteLine("您的成绩是 {0}", grade);
            //Console.ReadLine();
        }
        
    }
    class StaticVar
    {
        public static int num;
        public void count()
        {
            num++;
        }
        public static int getNum()
        {
            return num;
        }
    }
    class Rectangle
    {
        // 成员变量
        protected double length;
        protected double width;
        public Rectangle(double l, double w)
        {
            length = l;
            width = w;
        }
        public double GetArea()
        {
            return length * width;
        }
        public void Display()
        {
            Console.WriteLine("长度： {0}", length);
            Console.WriteLine("宽度： {0}", width);
            Console.WriteLine("面积： {0}", GetArea());
        }
    }//end class Rectangle  
    class Tabletop : Rectangle
    {
        private double cost;
        public Tabletop(double l, double w) : base(l, w)
        { }
        public double GetCost()
        {
            double cost;
            cost = GetArea() * 70;
            return cost;
        }
        public void Display()
        {
            base.Display();
            Console.WriteLine("成本： {0}", GetCost());
        }
    }
    public abstract class A
    {
        public int par { get; set; }
        public abstract void Test1();
    }
    interface B
    {
        public string str { get; set; }
        void Test2();
    }
    public class C : A, B
    {
        public string str { get; set; }
        public void Test2()
        {
            Console.WriteLine("实现了接口B的方法");
        }
        public override void Test1()
        {
            Console.WriteLine("重写了抽象类A的方法");
        }
        public virtual void Test3()
        {
            
        }
        
    }
    public class D : C
    {
        public void Test4()
        {
            Console.WriteLine("");
        }
    }
}
