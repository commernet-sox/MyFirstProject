using AspectCore.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SDT.BaseTool
{
    public static class ServiceContextExtensions
    {
        #region Members
        public static ConcurrentDictionary<string, List<string>> Container { get; private set; } = new ConcurrentDictionary<string, List<string>>();
        #endregion

        #region Public Methods
        #region AddType
        public static IServiceContext AddType<TService, TImplementation>(this IServiceContext serviceContext, string key, Lifetime lifetime = Lifetime.Scoped) where TImplementation : TService => serviceContext.AddType(typeof(TService), typeof(TImplementation), key, lifetime);

        public static IServiceContext AddType<TService>(this IServiceContext serviceContext, string key, Lifetime lifetime = Lifetime.Scoped) => serviceContext.AddType(typeof(TService), key, lifetime);

        public static IServiceContext AddType(this IServiceContext serviceContext, Type serviceType, Type implementationType, string key, Lifetime lifetime = Lifetime.Scoped)
        {
            var result = serviceContext.AddType(serviceType, implementationType, lifetime);
            serviceType.AddContainer(key);
            return result;
        }

        public static IServiceContext AddType(this IServiceContext serviceContext, Type serviceType, string key, Lifetime lifetime = Lifetime.Scoped) => serviceContext.AddType(serviceType, serviceType, key, lifetime);
        #endregion

        #region AddInstance
        public static IServiceContext AddInstance<TService>(this IServiceContext serviceContext, TService implementation, string key) => serviceContext.AddInstance(typeof(TService), implementation, key);

        public static IServiceContext AddInstance(this IServiceContext serviceContext, Type serviceType, object implementation, string key)
        {
            var result = serviceContext.AddInstance(serviceType, implementation);
            serviceType.AddContainer(key);
            return result;
        }
        #endregion

        #region AddDelegate
        public static IServiceContext AddDelegate(this IServiceContext serviceContext, Type serviceType, Func<IServiceResolver, object> implementationDelegate, string key, Lifetime lifetime = Lifetime.Transient)
        {
            var result = serviceContext.AddDelegate(serviceType, implementationDelegate, lifetime);
            serviceType.AddContainer(key);
            return result;
        }

        public static IServiceContext AddDelegate<TService>(this IServiceContext serviceContext, Func<IServiceResolver, TService> implementationDelegate, string key, Lifetime lifetime = Lifetime.Transient) where TService : class => serviceContext.AddDelegate(typeof(TService), implementationDelegate, key, lifetime);

        public static IServiceContext AddDelegate<TService, TImplementation>(this IServiceContext serviceContext, Func<IServiceResolver, TImplementation> implementationDelegate, string key, Lifetime lifetime = Lifetime.Transient) where TService : class where TImplementation : class, TService => serviceContext.AddDelegate(typeof(TService), implementationDelegate, key, lifetime);
        #endregion

        #region Resolve
        public static T Resolve<T>(this IServiceResolver serviceResolver, string key) where T : class
        {
            if (key.IsNull())
            {
                return serviceResolver.Resolve<T>();
            }

            var typeName = typeof(T)?.FullName ?? string.Empty;
            if (!Container.TryGetValue(typeName, out var list))
            {
                return null;
            }


            var index = list.FindIndex(t => t == key);
            if (index < 0)
            {
                return null;
            }

            if (list.Count() == 1)
            {
                return serviceResolver.Resolve<T>();
            }

            var array = serviceResolver.ResolveMany<T>();
            if (index >= array.Count())
            {
                return null;
            }

            return array.ToArray()[index];
        }

        public static T[] ResolveAll<T>(this IServiceResolver serviceResolver, string key = "")
        {
            if (key.IsNull())
            {
                return serviceResolver.ResolveMany<T>().ToArray();
            }

            var typeName = typeof(T)?.FullName ?? string.Empty;
            if (!Container.TryGetValue(typeName, out var list))
            {
                return null;
            }

            var indexList = list.FindArrayIndex(t => t == key);
            if (indexList.IsNull())
            {
                return null;
            }

            var array = serviceResolver.ResolveMany<T>().ToArray();
            var res = new List<T>();
            foreach (var index in indexList)
            {
                if (index >= array.Length)
                {
                    continue;
                }
                res.Add(array[index]);
            }
            return res.ToArray();
        }
        #endregion

        #region TryAdd
        public static void TryAdd(this IServiceContext serviceContext, ServiceDefinition serviceDefinition)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            if (serviceDefinition == null)
            {
                throw new ArgumentNullException(nameof(serviceDefinition));
            }

            if (!serviceContext.Any(t => t.ServiceType == serviceDefinition.ServiceType))
            {
                serviceContext.Add(serviceDefinition);
            }
        }

        public static void TryAddType(this IServiceContext serviceContext, Type serviceType, Lifetime lifetime = Lifetime.Transient) => serviceContext.TryAddType(serviceType, serviceType, lifetime);

        public static void TryAddType(this IServiceContext serviceContext, Type serviceType, Type implementationType, Lifetime lifetime = Lifetime.Transient)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            if (!serviceContext.CheckServiceType(serviceType))
            {
                serviceContext.AddType(serviceType, implementationType, lifetime);
            }
        }

        public static void TryAddType<TService>(this IServiceContext serviceContext, Lifetime lifetime = Lifetime.Transient) => serviceContext.TryAddType(typeof(TService), lifetime);

        public static void TryAddType<TService, TImplementation>(this IServiceContext serviceContext, Lifetime lifetime = Lifetime.Transient)
            where TImplementation : TService => serviceContext.TryAddType(typeof(TService), typeof(TImplementation), lifetime);

        public static void TryAddInstance(this IServiceContext serviceContext, Type serviceType, object implementationInstance)
        {
            if (!serviceContext.CheckServiceType(serviceType))
            {
                serviceContext.AddInstance(serviceType, implementationInstance);
            }
        }

        public static void TryAddInstance<TService>(this IServiceContext serviceContext, TService implementationInstance) => serviceContext.TryAddInstance(typeof(TService), implementationInstance);

        public static void TryAddDelegate(this IServiceContext serviceContext, Type serviceType, Func<IServiceResolver, object> implementationDelegate, Lifetime lifetime = Lifetime.Transient)
        {
            if (!serviceContext.CheckServiceType(serviceType))
            {
                serviceContext.AddDelegate(serviceType, implementationDelegate, lifetime);
            }
        }

        public static void TryAddDelegate<TService, TImplementation>(this IServiceContext serviceContext, Func<IServiceResolver, TImplementation> implementationDelegate, Lifetime lifetime = Lifetime.Transient)
            where TService : class
            where TImplementation : class, TService => serviceContext.TryAddDelegate(typeof(TService), implementationDelegate, lifetime);

        public static void TryAddDelegate<TService>(this IServiceContext serviceContext, Func<IServiceResolver, TService> implementationDelegate, Lifetime lifetime = Lifetime.Transient) where TService : class => serviceContext.TryAddDelegate(typeof(TService), implementationDelegate, lifetime);
        #endregion

        #region Register
        public static IServiceContext AddAssemblyTypes(this IServiceContext serviceContext, Type serviceType, Lifetime? lifetime, params Assembly[] assemblies)
        {
            if (assemblies.IsNull())
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            var types = TypeFinderUtility.FindClassesOfType(serviceType, assemblies);

            foreach (var type in types)
            {
                var ignore = type.GetCustomAttribute<DependencyIgnoreAttribute>(false);
                if (ignore != null)
                {
                    continue;
                }

                var tLifetime = Lifetime.Transient;
                if (lifetime.HasValue)
                {
                    tLifetime = lifetime.Value;
                }
                else
                {
                    var dep = type.GetCustomAttribute<DependencyPropertyAttribute>(true);
                    if (dep != null)
                    {
                        tLifetime = dep.Lifetime;
                    }
                }

                if (serviceType.IsGenericType)
                {
                    if (!serviceType.IsInterface)
                    {
                        if (type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == serviceType)
                        {
                            serviceContext.AddType(type.BaseType, type, tLifetime);
                        }
                        else if (type.IsGenericType && type.GetGenericTypeDefinition() == serviceType)
                        {
                            serviceContext.AddType(type, tLifetime);
                        }
                    }
                    else
                    {
                        var inters = type.GetInterfaces();
                        foreach (var iType in inters)
                        {
                            if (iType.IsGenericType && iType.GetGenericTypeDefinition() == serviceType)
                            {
                                serviceContext.AddType(iType, type, tLifetime);
                                continue;
                            }
                        }
                    }

                    continue;
                }
                serviceContext.AddType(serviceType, type, tLifetime);
            }

            return serviceContext;
        }

        public static IServiceContext AddAssemblyTypes(this IServiceContext serviceContext, Type serviceType, params Assembly[] assemblies) => serviceContext.AddAssemblyTypes(serviceType, null, assemblies);

        public static IServiceContext AddAssemblyTypes<T>(this IServiceContext serviceContext, params Assembly[] assemblies) => serviceContext.AddAssemblyTypes<T>(null, assemblies);

        public static IServiceContext AddAssemblyTypes<T>(this IServiceContext serviceContext, Lifetime? lifetime, params Assembly[] assemblies) => serviceContext.AddAssemblyTypes(typeof(T), lifetime, assemblies);
        #endregion

        #region EngineService
        public static IServiceContext AddEngineService<T>(this IServiceContext serviceContext)
            where T : class, IEngineService => serviceContext.AddType<IEngineService, T>(Lifetime.Transient);

        public static IServiceContext AddEngineService<T>(this IServiceContext serviceContext, Func<IServiceResolver, T> implementationFactory)
            where T : class, IEngineService => serviceContext.AddDelegate<IEngineService>(implementationFactory, Lifetime.Transient);
        #endregion
        #endregion

        #region Private Methods
        private static void AddContainer(this Type type, string key)
        {
            if (key.IsNull())
            {
                return;
            }

            var typeName = type?.FullName ?? string.Empty;
            if (Container.TryGetValue(typeName, out var list))
            {
                list.Add(key);
            }
            else
            {
                list = new List<string> { key };
                Container[typeName] = list;
            }
        }

        private static bool CheckServiceType(this IServiceContext serviceContext, Type serviceType)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            return serviceContext.Any(t => t.ServiceType == serviceType);
        }
        #endregion
    }
}
