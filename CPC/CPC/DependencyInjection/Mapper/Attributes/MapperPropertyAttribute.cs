using System;

namespace CPC
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
