using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CPC
{
    public static class TypeFinderUtility
    {
        #region Public Methods
        public static IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Type> types, bool onlyConcreteClasses = true)
        {
            var result = new List<Type>();

            if (types.IsNull())
            {
                return result;
            }

            foreach (var t in types)
            {
                if (assignTypeFrom.IsAssignableFrom(t) || (assignTypeFrom.IsGenericTypeDefinition && DoesTypeImplementOpenGeneric(t, assignTypeFrom)))
                {
                    if (!t.IsInterface)
                    {
                        if (onlyConcreteClasses)
                        {
                            if (t.IsClass && !t.IsAbstract)
                            {
                                result.Add(t);
                            }
                        }
                        else
                        {
                            result.Add(t);
                        }
                    }
                }
            }

            return result;
        }

        public static IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
        {
            var result = new List<Type>();
            if (assignTypeFrom == null || assemblies.IsNull())
            {
                return result;
            }

            try
            {
                foreach (var a in assemblies)
                {
                    var types = a.GetTypes();
                    result.AddRange(FindClassesOfType(assignTypeFrom, types, onlyConcreteClasses));
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                var msg = string.Empty;
                foreach (var e in ex.LoaderExceptions)
                {
                    msg += e.Message + Environment.NewLine;
                }

                var fail = new Exception(msg, ex);
                throw fail;
            }

            return result;
        }

        public static IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true) => FindClassesOfType(typeof(T), assemblies, onlyConcreteClasses);

        public static IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<string> assemblyNames, bool onlyConcreteClasses = true) => FindClassesOfType(assignTypeFrom, GetAssemblies(assemblyNames), onlyConcreteClasses);

        public static IEnumerable<Type> FindClassesOfType<T>(IEnumerable<string> assemblyNames, bool onlyConcreteClasses = true) => FindClassesOfType(typeof(T), assemblyNames, onlyConcreteClasses);

        public static IEnumerable<Assembly> GetAssemblies(IEnumerable<string> assemblyNames)
        {
            var assemblies = new List<Assembly>();
            foreach (var assemblyName in assemblyNames)
            {
                var ass = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyName);
                var assembly = Assembly.LoadFrom(ass);
                if (!assemblies.Any(t => t.FullName == assembly.FullName))
                {
                    assemblies.Add(assembly);
                }
            }

            return assemblies;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// does type implement generic
        /// </summary>
        /// <param name="type"></param>
        /// <param name="openGeneric"></param>
        /// <returns></returns>
        private static bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric)
        {
            try
            {
                var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();
                if (genericTypeDefinition.IsInterface)
                {
                    var types = type.FindInterfaces((objType, objCriteria) => true, null);
                    foreach (var implementedInterface in types)
                    {
                        if (!implementedInterface.IsGenericType)
                        {
                            continue;
                        }

                        var isMatch = genericTypeDefinition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition());

                        if (!isMatch)
                        {
                            continue;
                        }

                        return isMatch;
                    }
                    return false;
                }
                else if (type.BaseType != null)
                {
                    if (!type.BaseType.IsGenericType)
                    {
                        return false;
                    }

                    var isMatch = genericTypeDefinition.IsAssignableFrom(type.BaseType.GetGenericTypeDefinition());

                    return isMatch;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
