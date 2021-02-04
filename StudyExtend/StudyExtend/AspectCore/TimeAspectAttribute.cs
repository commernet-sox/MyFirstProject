using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace StudyExtend.AspectCore
{
    /// <summary>
    /// 计算方法执行时间
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TimeAspectAttribute : AbstractInterceptorAttribute
    {
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var sw = Stopwatch.StartNew();
            await next(context);
            var item = context.ReturnValue;
            Type type = context.ProxyMethod.ReturnType;
            Console.WriteLine($"方法返回值：{item} 类型为{type}");
            sw.Stop();
            Console.WriteLine($"method {context.ProxyMethod.Name} in {sw.ElapsedMilliseconds} ms");
            
        }
    }
}
