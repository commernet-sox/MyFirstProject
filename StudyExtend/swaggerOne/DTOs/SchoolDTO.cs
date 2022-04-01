using System;
using System.Collections.Generic;
using System.Text;
using CPC.DependencyInjection;
namespace swaggerOne.DTOs
{
    public class SchoolDTO:IMapDto
    {
        public string Name { get; set; }
        public string Desc { get; set; }
    }
}
