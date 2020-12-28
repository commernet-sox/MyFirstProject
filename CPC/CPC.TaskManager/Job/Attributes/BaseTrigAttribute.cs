using Quartz;
using System;

namespace CPC.TaskManager
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class BaseTrigAttribute : Attribute
    {
        /// <summary>
        /// 触发器基本信息
        /// </summary>
        public Structure BaseInfo { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int? Priority { get; set; }

        /// <summary>
        /// 日历名称
        /// </summary>
        public string CalendarName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTimeOffset? StartAt { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTimeOffset? EndAt { get; set; }

        /// <summary>
        /// 创建触发器
        /// </summary>
        /// <returns></returns>
        public virtual TriggerBuilder CreateTrigger()
        {
            BaseInfo = BaseInfo ?? new Structure(RandomUtility.GuidString());
            var builder = TriggerBuilder.Create().WithIdentity(BaseInfo.ToTriggerKey()).WithDescription(BaseInfo.Description).UsingJobData(BaseInfo.ToDataMap());
            if (Priority.HasValue)
            {
                builder.WithPriority(Priority.Value);
            }

            if (StartAt.HasValue)
            {
                builder.StartAt(StartAt.Value);
            }
            else
            {
                builder.StartNow();
            }

            if (EndAt.HasValue)
            {
                builder.EndAt(EndAt.Value);
            }

            if (!CalendarName.IsNull())
            {
                builder.ModifiedByCalendar(CalendarName);
            }

            return builder;
        }
    }
}
