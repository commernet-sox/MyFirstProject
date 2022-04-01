using swagger.Interfaces;
using System;
using CPC.DependencyInjection;

namespace swagger.Services
{
    public class EmployService : IEmploy, IAutoDepends
    {
        public void Name(string name)
        {

            Console.WriteLine(name);
        }
    }
}
