using System;
using System.Collections.Generic;
using System.Text;

namespace SDT.BaseTool
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MapperPropertyAttribute : Attribute
    {
        public MapperPropertyAttribute()
        {

        }

        public string MapName { get; set; }

        public Type MapType { get; set; }
    }
}
