using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CPC.TaskManager
{
    public class CronUtility
    {
        public static bool ValidExpression(string cronExpression) => CronExpression.IsValidExpression(cronExpression);

        public static List<DateTime> ComputeFireTimes(string cronExpression, int numTimes)
        {
            var trigger = TriggerBuilder.Create().WithCronSchedule(cronExpression).Build();
            var dates = TriggerUtils.ComputeFireTimes(trigger as IOperableTrigger, null, numTimes);
            var list = dates.Select(dtf => TimeZoneInfo.ConvertTimeFromUtc(dtf.DateTime, TimeZoneInfo.Local)).ToList();
            return list;
        }

    }
}
