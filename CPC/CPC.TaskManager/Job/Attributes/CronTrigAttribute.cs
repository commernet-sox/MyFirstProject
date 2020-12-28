using Quartz;
using System;
using static Quartz.MisfireInstruction;

namespace CPC.TaskManager
{
    public class CronTrigAttribute : BaseTrigAttribute
    {
        /// <summary>
        /// 时区
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// 触发失败机制
        /// </summary>
        public int MisfireInstruction { get; set; }

        public override TriggerBuilder CreateTrigger()
        {
            var builder = base.CreateTrigger();
            builder.WithCronSchedule(Expression, c =>
            {
                if (!TimeZone.IsNull())
                {
                    c.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById(TimeZone));
                }

                switch (MisfireInstruction)
                {
                    case -1:
                        c.WithMisfireHandlingInstructionIgnoreMisfires();
                        break;
                    case 0:
                        break;
                    case CronTrigger.FireOnceNow:
                        c.WithMisfireHandlingInstructionFireAndProceed();
                        break;
                    case CronTrigger.DoNothing:
                        c.WithMisfireHandlingInstructionDoNothing();
                        break;
                    default: throw new ArgumentException(nameof(MisfireInstruction));
                }
            });

            return builder;
        }
    }
}
