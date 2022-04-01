using System;
using System.Collections.Generic;
using System.Linq;
using CPC;

namespace ExpressionTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            List<Emp> emps = new List<Emp>();
            foreach (var item in Enumerable.Range(1, 100))
            {
                Emp emp = new Emp();
                emp.Name = item.ToString();
                emp.Age = item;
                emp.Desc = item.ToString();
                emps.Add(emp);
            }
            //var empExp = ExprBuilder.True<Emp>();
            //var express = ExprBuilder.Create<Emp>("Name","1",ExprOperator.Equal);
            //var express1 = ExprBuilder.Create<Emp>("Name","2",ExprOperator.Equal);
            //var res = emps.Where(express.Or<Emp>(express1).And<Emp>(p=>p.Name, "1").Compile()).ToList();

            ExpreBuilder<Emp> builder = new ExpreBuilder<Emp>();
            var empExp = ExprBuilder.Create<Emp>("Name", "12", ExprOperator.Equal);
            var expreBuilder = builder.Property("Name", empExp).Ignore("Age").Generate(new Emp() { Age = 12 });
            var res = emps.Where(expreBuilder.Compile()).WhereIf(true,t=>t.Desc!="11").ToList();
            foreach (var item in res)
            {
                Console.WriteLine(item.Name);
            }


        }
    }

    public class Emp
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Desc { get; set; }
    }
}
