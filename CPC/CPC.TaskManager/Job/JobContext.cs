using Quartz;
using System;

namespace CPC.TaskManager
{
    /// <summary>
    /// 任务配置，必须传值的为：CronExpression (AssName+JobPath or JobType)
    /// </summary>
    public class JobContext
    {
        #region Members
        /// <summary>
        /// 作业运行对象
        /// </summary>
        public Structure Job { get; set; }

        public string CronExpression { get; set; }

        /// <summary>
        /// 指定的Job Type Name
        /// </summary>
        public string JobPath { get; set; } = string.Empty;

        /// <summary>
        /// Job Type（JobPath 与其二选一填写）
        /// </summary>
        public Type JobType { get; set; }

        public bool Recovery { get; set; } = false;

        /// <summary>
        /// 此任务是否启用，默认为True
        /// </summary>
        public bool IsActive { get; set; } = true;

        public JobTriggers Trigger { get; } = new JobTriggers();
        #endregion

        #region Methods
        public virtual JobBuilder CreateJob()
        {
            var job = JobBuilder.Create(JobType).WithIdentity(Job.ToJobKey()).WithDescription(Job.Description).SetJobData(Job.ToDataMap()).RequestRecovery(Recovery).StoreDurably();
            return job;
        }
        #endregion
    }
}
