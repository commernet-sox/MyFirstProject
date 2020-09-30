using AspectCore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SDT.BaseTool
{
    public class AutoDependencyRegistrar : IDependencyRegistrar
    {
        public int Order => int.MaxValue;
        protected readonly string[] _assemblyNames;

        public AutoDependencyRegistrar(string[] assemblyNames) => _assemblyNames = assemblyNames;

        public void Register(IServiceContext service)
        {
            var types = TypeFinderUtility.FindClassesOfType<IAutoDependency>(_assemblyNames);
            foreach (var type in types)
            {
                var ignore = type.GetCustomAttribute<DependencyIgnoreAttribute>(false);
                if (ignore != null)
                {
                    continue;
                }

                var serviceTypes = new List<Type>();
                var lifetime = Lifetime.Transient;

                var dep = type.GetCustomAttribute<DependencyPropertyAttribute>(true);
                if (dep != null)
                {
                    lifetime = dep.Lifetime;

                    if (dep.Services != null)
                    {
                        serviceTypes = dep.Services.ToList();
                    }
                }

                if (serviceTypes.IsNull())
                {
                    if (type.BaseType != null && type.BaseType != typeof(object))
                    {
                        serviceTypes.Add(type.BaseType);
                    }

                    var inters = type.GetInterfaces();
                    if (!inters.IsNull())
                    {
                        foreach (var inter in inters)
                        {
                            if (inter.Name == nameof(IAutoDependency))
                            {
                                continue;
                            }
                            serviceTypes.Add(inter);
                        }
                    }

                    if (serviceTypes.IsNull())
                    {
                        serviceTypes.Add(type);
                    }
                }

                foreach (var serviceType in serviceTypes)
                {
                    if (serviceType.Name != type.Name)
                    {
                        service.AddType(serviceType, type, lifetime);
                    }
                    else
                    {
                        service.AddType(type, lifetime);
                    }
                }
            }
        }
    }
}
