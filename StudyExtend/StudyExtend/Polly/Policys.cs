using Polly;
using Polly.Timeout;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static StudyExtend.Program;

namespace StudyExtend.Polly
{
    public class Policys
    {
        public void Test()
        {
            //定义超时策略
            var timeoutPolicy = Policy.TimeoutAsync(1, (context, timespan, task) =>
            {
                Console.WriteLine("超时了，抛出TimeoutRejectedException异常。");
                return Task.CompletedTask;
            });


            //定义重试策略
            var retryPolicy = Policy.Handle<HttpRequestException>().Or<TimeoutException>().Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(
            retryCount: 2,
            sleepDurationProvider: retryAttempt =>
            {
                var waitSeconds = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt - 1));
                Console.WriteLine(DateTime.Now.ToString() + "----------------重试策略:[" + retryAttempt + "], 等待 " +   waitSeconds + "秒!");
                return waitSeconds;
            });


            //定义熔断策略
            var circuitBreakerPolicy = Policy.Handle<HttpRequestException>().Or<TimeoutException>().Or<TimeoutRejectedException>()
            .CircuitBreakerAsync(
            // 熔断前允许出现几次错误
            exceptionsAllowedBeforeBreaking: 2,
            // 熔断时间
            durationOfBreak: TimeSpan.FromSeconds(3),
            // 熔断时触发
            onBreak: (ex, breakDelay) =>
            {
                Console.WriteLine(DateTime.Now.ToString() + "----------------断路器->断路 " + breakDelay.TotalMilliseconds +  "毫秒! 异常: ", ex.Message);
            },
            // 熔断恢复时触发
            onReset: () =>
            {
                Console.WriteLine(DateTime.Now.ToString() + "----------------断路器->好了! 再次闭合电路.");
            },
            // 在熔断时间到了之后触发
            onHalfOpen: () =>
            {
                Console.WriteLine(DateTime.Now.ToString() + "----------------断路器->半开，下一个呼叫是测试.");
            }
            );


            //定义降级策略
            var fallbackPolicy = Policy<string>.Handle<Exception>()
            .FallbackAsync(
               fallbackValue: "替代数据",
               onFallbackAsync: (exception, context) =>
               {
                   Console.WriteLine("降级策略,  异常->" + exception.Exception.Message + ", 返回替代数据.");
                   return Task.CompletedTask;
               });
            //循环执行50次，策略的执行是非常简单的，唯一需要注意的就是调用的顺序：如下是依次从右到左进行调用，
            //首先是进行超时的判断，一旦超时就触发TimeoutRejectedException异常，
            //然后就进入到了重试策略中，如果重试了一次就成功了，那就直接返回，不再触发其他策略，
            //否则就进入到熔断策略中：
            Task.Run(async () =>
            {
                for (int i = 0; i < 20; i++)
                {
                    Console.WriteLine(DateTime.Now.ToString() + "----------------开始第[" + i + "]-----------------------------");
                    var res = await fallbackPolicy
                    .WrapAsync(Policy.WrapAsync(circuitBreakerPolicy, retryPolicy, timeoutPolicy))
                    .ExecuteAsync(() => new PollyTest().HttpInvokeAsync());
                    Console.WriteLine(DateTime.Now.ToString() + "--------------- 开始[" + i + "]->响应" + ": Ok->" + res);
                    await Task.Delay(1000);
                    Console.WriteLine("--------------------------------------------------------------------------------------------------------------------");
                }
            });


        }
    }
}
