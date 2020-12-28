using Quartz;
using System;
using static Quartz.MisfireInstruction;

namespace CPC.TaskManager
{
    public class SimpleTrigAttribute : BaseTrigAttribute
    {
        /// <summary>
        /// 重复时间间隔（s）
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// 重复次数
        /// </summary>
        public int RepeatCount { get; set; } = -1;

        /// <summary>
        /// 触发失败机制
        /// </summary>
        public int MisfireInstruction { get; set; }

        public override TriggerBuilder CreateTrigger()
        {
            var builder = base.CreateTrigger();
            builder.WithSimpleSchedule(s =>
            {
                s.WithInterval(TimeSpan.FromSeconds(Interval));
                if (RepeatCount > 0)
                {
                    s.WithRepeatCount(RepeatCount);
                }
                else
                {
                    s.RepeatForever();
                }

                switch (MisfireInstruction)
                {
                    case -1:
                        s.WithMisfireHandlingInstructionIgnoreMisfires();
                        break;
                    case 0:
                        break;
                    case SimpleTrigger.FireNow:
                        s.WithMisfireHandlingInstructionFireNow();
                        break;
                    case SimpleTrigger.RescheduleNowWithExistingRepeatCount:
                        s.WithMisfireHandlingInstructionNowWithExistingCount();
                        break;
                    case SimpleTrigger.RescheduleNowWithRemainingRepeatCount:
                        s.WithMisfireHandlingInstructionNowWithRemainingCount();
                        break;
                    case SimpleTrigger.RescheduleNextWithRemainingCount:
                        s.WithMisfireHandlingInstructionNowWithRemainingCount();
                        break;
                    case SimpleTrigger.RescheduleNextWithExistingCount:
                        s.WithMisfireHandlingInstructionNextWithExistingCount();
                        break;
                    default: throw new ArgumentException(nameof(MisfireInstruction));
                }
            });

            return builder;
        }
    }
}
