using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPC.TaskManager
{
    public class TaskProvider
    {
        #region Members
        private JobKey _jobKey;
        #endregion

        #region Constructors
        public TaskProvider(JobKey key)
        {
            if (TaskPool.Scheduler == null)
            {
                throw new Exception("task scheduler not running");
            }

            _jobKey = key ?? throw new Exception("jobKey is null");
        }
        #endregion

        #region Methods
        /// <summary>
        /// 删除作业
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteJobAsync()
        {
            var result = await TaskPool.Scheduler.DeleteJob(_jobKey);
            return result;
        }

        /// <summary>
        /// 删除当前作业指定的触发器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task<bool> DeleteTriggerAsync(string name, string group)
        {
            var key = new TriggerKey(name, group);
            var result = await TaskPool.Scheduler.UnscheduleJob(key);
            return result;
        }

        /// <summary>
        /// 暂定作业
        /// </summary>
        /// <returns></returns>
        public async Task PauseJobAsync() => await TaskPool.Scheduler.PauseJob(_jobKey);

        /// <summary>
        /// 恢复作业
        /// </summary>
        /// <returns></returns>
        public async Task ResumeJobAsync() => await TaskPool.Scheduler.ResumeJob(_jobKey);

        /// <summary>
        /// 暂停作业指定触发器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task PauseTriggerAsync(string name, string group) => await TaskPool.Scheduler.PauseTrigger(new TriggerKey(name, group));

        /// <summary>
        /// 恢复作业指定触发器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task ResumeTriggerAsync(string name, string group) => await TaskPool.Scheduler.ResumeTrigger(new TriggerKey(name, group));

        /// <summary>
        /// 立即执行当前作业
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public async Task<bool> ExecutionAsync(IDictionary<string, object> map = null)
        {
            if (map == null)
            {
                map = new Dictionary<string, object>();
            }

            await TaskPool.Scheduler.TriggerJob(_jobKey, new JobDataMap(map));
            return true;
        }

        /// <summary>
        /// 更新当前作业
        /// </summary>
        /// <param name="newJob"></param>
        /// <returns></returns>
        public async Task UpdateJobAsync(JobContext newJob)
        {
            if (!newJob.Trigger.TriggerCaches.IsNull())
            {
                throw new InvalidOperationException("can't change the trigger for this job");
            }

            var triggers = await TaskPool.Scheduler.GetTriggersOfJob(_jobKey);

            TaskPool.PostConfigure(newJob);

            // set new job to all triggers
            triggers = triggers.Select(t =>
            {
                var b = t.GetTriggerBuilder().ForJob(newJob.Job.ToJobKey());
                if (t.StartTimeUtc < DateTimeOffset.UtcNow)
                {
                    b.StartNow();
                }

                return b.Build();
            }).ToArray();

            // delete old job
            await TaskPool.Scheduler.DeleteJob(_jobKey);

            var jobDetail = newJob.CreateJob().Build();
            // save new job with triggers
            await TaskPool.Scheduler.ScheduleJob(jobDetail, triggers, true);
            _jobKey = newJob.Job.ToJobKey();
        }

        /// <summary>
        /// 对于当前作业添加触发器
        /// </summary>
        /// <param name="triggers"></param>
        /// <returns></returns>
        public async Task AddTriggersAsync(JobTriggers triggers)
        {
            if (triggers.TriggerCaches.IsNull())
            {
                throw new Exception("no trigger");
            }

            foreach (var tb in triggers.TriggerCaches)
            {
                var trigger = tb.ForJob(_jobKey).Build();
                await TaskPool.Scheduler.ScheduleJob(trigger);
            }
            triggers.ClearTrigger();
        }
        #endregion
    }
}
