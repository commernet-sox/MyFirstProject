using AspectCore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDT.BaseTool
{
    public static class GlobalContext
    {
        public static IServiceResolver CreateScope() => EngineContext.Current.Container.CreateScope();

        public static T Resolve<T>(string key = "")
            where T : class
            => CreateScope().Resolve<T>(key);

        public static object Resolve(Type type) => CreateScope().Resolve(type);

        public static T[] ResolveAll<T>(string key = "") => CreateScope().ResolveAll<T>(key);

        public static T ResolveUnregistered<T>()
            where T : class
            => ResolveUnregistered(typeof(T)) as T;

        public static object ResolveUnregistered(Type type)
        {
            var scope = CreateScope();
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new List<object>();
                    foreach (var parameter in parameters)
                    {
                        var service = scope.Resolve(parameter.ParameterType);
                        if (service == null)
                        {
                            throw new Exception("Unknown dependency");
                        }

                        parameterInstances.Add(service);
                    }

                    return Activator.CreateInstance(type, parameterInstances.ToArray());
                }
                catch (Exception)
                {
                }
            }

            throw new Exception("No constructor was found that had all the dependencies satisfied.");
        }

        public static object TryResolve(Type type)
        {
            object result;
            if (type.IsConcrete())
            {
                if (EngineContext.Initialized)
                {
                    result = ResolveUnregistered(type);
                }
                else
                {
                    result = Activator.CreateInstance(type);
                }
            }
            else
            {
                result = Resolve(type);
            }

            return result;
        }

        public static T TryResolve<T>()
            where T : class
            => TryResolve(typeof(T)) as T;
    }
}
