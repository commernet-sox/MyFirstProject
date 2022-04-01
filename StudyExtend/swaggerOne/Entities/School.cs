using System;
using System.Collections.Generic;
using System.Text;
using CPC.DependencyInjection;

namespace swaggerOne.Entities
{
    public class School:IMapEntity
    {
        public string Name { get; set; }
        public string Desc { get; set; }
    }
}
