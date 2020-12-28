using Quartz;
using System;

namespace CPC.TaskManager
{
    public static class JobExtensions
    {
        public static JobTriggers AddCronTrigger(this JobTriggers context, string cron, Structure trigger = null, Action<TriggerBuilder> triggerSetup = null)
        {
            if (!CronUtility.ValidExpression(cron))
            {
                throw new Exception("invalid cron expression");
            }

            return context.AddTrigger(trigger, tb =>
            {
                var t = tb.WithCronSchedule(cron).StartNow();
                triggerSetup?.Invoke(t);
            });
        }


    }
}
