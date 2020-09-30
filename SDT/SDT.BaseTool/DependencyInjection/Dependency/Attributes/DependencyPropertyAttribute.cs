using AspectCore.DependencyInjection;
using System;

namespace SDT.BaseTool
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DependencyPropertyAttribute : Attribute
    {
        public DependencyPropertyAttribute(Type service = null)
        {
            if (service != null)
            {
                Services = new Type[] { service };
            }
        }

        public Type[] Services { get; set; }

        public Lifetime Lifetime { get; set; } = Lifetime.Transient;
    }
}
