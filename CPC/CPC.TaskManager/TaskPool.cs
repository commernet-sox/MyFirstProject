using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CPC.TaskManager
{
    public class TaskPool
    {
        #region Members
        public static IScheduler Scheduler { get; private set; }

        /// <summary>
        /// 清空外部持久化数据
        /// </summary>
        private static bool _clearStore = false;
        #endregion

        #region Methods
        public static void ClearStore() => _clearStore = true;

        public static Task StartAsync(params string[] assemblyNames) => StartAsync(TypeFinderUtility.GetAssemblies(assemblyNames).ToArray());

        public static async Task StartAsync(params Assembly[] assemblies)
        {
            if (assemblies.IsNull())
            {
                throw new ArgumentException("no assemblies found to scan. Supply at least one assembly to scan for handlers.");
            }

            var types = TypeFinderUtility.FindClassesOfType<IJob>(assemblies);
            var list = new List<JobContext>();
            if (!types.IsNull())
            {
                foreach (var type in types)
                {
                    var trigAttrs = type.GetCustomAttributes<BaseTrigAttribute>(true);
                    if (trigAttrs.IsNull())
                    {
                        continue;
                    }

                    var job = new JobContext() { JobType = type };

                    var jobAttr = type.GetCustomAttribute<JobPropertyAttribute>(false);
                    if (jobAttr != null)
                    {
                        job.Job = jobAttr.BaseInfo;
                        job.Recovery = jobAttr.Recovery;
                        job.IsActive = jobAttr.IsActive;
                    }

                    foreach (var trig in trigAttrs)
                    {
                        job.Trigger.AddTrigger(trig.CreateTrigger());
                    }
                    list.Add(job);
                }
            }
            await StartAsync(list);
        }

        public static Task StartAsync(params JobContext[] jobContexts) => StartAsync(jobContexts, null);


        public static async Task StartAsync(IEnumerable<JobContext> jobContexts, Action<NameValueCollection> propsSetup = null)
        {
            if (jobContexts.IsNull())
            {
                throw new Exception("jobSettings is null");
            }

            if (Scheduler != null)
            {
                throw new Exception("task scheduler is running");
            }


            StdSchedulerFactory sf;
            var props = new NameValueCollection();
            ExecPlugin(p => p.OnInitialize(props));
            propsSetup?.Invoke(props);

            if (props.IsNull() || props.Count == 0)
            {
                sf = new StdSchedulerFactory();
            }
            else
            {
                sf = new StdSchedulerFactory(props);
            }

            Scheduler = await sf.GetScheduler();

            if (_clearStore)
            {
                await Scheduler.Clear();
            }

            await AddAsync(jobContexts, false);

            await Scheduler.Start();
            ExecPlugin(p => p.OnStart());
        }

        private static void ExecPlugin(Action<TaskPlugin> setup)
        {
            if (EngineContext.Initialized)
            {
                var plugins = GlobalContext.ResolveAll<TaskPlugin>();
                if (!plugins.IsNull())
                {
                    foreach (var plugin in plugins)
                    {
                        setup?.Invoke(plugin);
                    }
                }
            }
        }

        internal static void PostConfigure(in JobContext context)
        {
            if (context.JobType != null)
            {
                context.JobPath = context.JobType.GetTypeName();
            }
            else
            {
                if (context.JobPath.IsNull())
                {
                    throw new Exception("no corresponding task specified");
                }

                context.JobType = Type.GetType(context.JobPath);
            }

            if (!context.CronExpression.IsNull())
            {
                context.Trigger.AddCronTrigger(context.CronExpression);
            }

            context.Job = context.Job ?? new Structure(context.JobType.Name);
        }

        public static Task AddAsync(params JobContext[] jobContexts) => AddAsync(jobContexts);

        private static async Task AddAsync(IEnumerable<JobContext> jobContexts, bool check = true)
        {
            if (jobContexts.IsNull())
            {
                throw new ArgumentNullException(nameof(jobContexts));
            }

            foreach (var context in jobContexts)
            {
                PostConfigure(context);

                if (await Scheduler.CheckExists(context.Job.ToJobKey()))
                {
                    if (!check)
                    {
                        continue;
                    }

                    throw new ArgumentException($"this job name:'{context.Job.Name}' group:'{context.Job.Group}' already exists");
                }

                var jobDetail = context.CreateJob().Build();
                await Scheduler.AddJob(jobDetail, false);

                if (!context.Trigger.TriggerCaches.IsNull())
                {
                    foreach (var tb in context.Trigger.TriggerCaches)
                    {
                        var trigger = tb.ForJob(context.Job.ToJobKey()).Build();
                        await Scheduler.ScheduleJob(trigger);
                    }
                    context.Trigger.ClearTrigger();
                }

                if (!context.IsActive)
                {
                    await Scheduler.PauseJob(context.Job.ToJobKey());
                }
            }
        }

        public static async Task ShutdownAsync()
        {
            if (Scheduler != null && !Scheduler.IsShutdown)
            {
                await Scheduler.Shutdown();
                ExecPlugin(p => p.OnShutdown());
            }
        }
        #endregion
    }
}
