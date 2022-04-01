using swaggerOne.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using CPC.DependencyInjection;

namespace swaggerOne.Services
{
    internal class SchoolServiec : ISchool,IAutoDepends
    {
        public string Name(string name)
        {
            return name;
        }
    }
}
