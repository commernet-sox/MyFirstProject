using AspectCore.DependencyInjection;
using AspectCore.DynamicProxy;
using CPC;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebApi.Application.Cache.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RedisCachingAttribute : AbstractInterceptorAttribute
    {
        /// <summary>
        /// 过期时间（分钟）,默认120分钟
        /// </summary>
        public double Expire { get; set; } = 120;

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var key = string.Concat(context.ServiceMethod.DeclaringType.FullName, ".", context.ServiceMethod.Name, "#", context.Parameters.JoinStr("_"));

            var redisCache = context.ServiceProvider.Resolve<ISeedCache>();
            var json = redisCache.GetString(key);
            if (json != null)
            {
                var result = JsonConvert.DeserializeObject(json, context.ServiceMethod.ReturnType, JsonHelper.CommonSetting);
                context.ReturnValue = result;
                return;
            }

            await next(context);
            var item = context.ReturnValue;
            redisCache.Add(key, item, TimeSpan.FromMinutes(Expire));
        }
    }
}
