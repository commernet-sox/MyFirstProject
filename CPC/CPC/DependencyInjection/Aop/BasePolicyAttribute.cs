using AspectCore.DynamicProxy;
using Polly;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace CPC
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class BasePolicyAttribute : AbstractInterceptorAttribute
    {
        protected static ConcurrentDictionary<MethodInfo, IAsyncPolicy> Polices { get; set; } = new ConcurrentDictionary<MethodInfo, IAsyncPolicy>();
    }
}
