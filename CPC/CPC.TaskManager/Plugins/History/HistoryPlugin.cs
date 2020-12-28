using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Spi;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CPC.TaskManager.Plugins
{
    public class HistoryPlugin : ISchedulerPlugin, IJobListener
    {
        #region Members
        private IHistoryStore _store;
        private IScheduler _scheduler;

        public string Name { get; set; }
        public Type StoreType { get; set; } = typeof(HistoryStore);
        #endregion

        #region JobListener
        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default) => Task.Run(() =>
                                                                                                                                 {
                                                                                                                                     var entry = _store.Get(context.FireInstanceId);
                                                                                                                                     if (entry != null)
                                                                                                                                     {
                                                                                                                                         entry.Vetoed = true;
                                                                                                                                         _store.Save(entry);
                                                                                                                                     }
                                                                                                                                 });

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default) => Task.Run(() =>
                                                                                                                              {
                                                                                                                                  var entry = new History()
                                                                                                                                  {
                                                                                                                                      FireInstanceId = context.FireInstanceId,
                                                                                                                                      SchedulerInstanceId = context.Scheduler.SchedulerInstanceId,
                                                                                                                                      SchedulerName = context.Scheduler.SchedulerName,
                                                                                                                                      ActualFireTimeUtc = context.FireTimeUtc.UtcDateTime,
                                                                                                                                      ScheduledFireTimeUtc = context.ScheduledFireTimeUtc?.UtcDateTime,
                                                                                                                                      Recovering = context.Recovering,
                                                                                                                                      Job = context.JobDetail.Key.ToString(),
                                                                                                                                      Trigger = context.Trigger.Key.ToString(),
                                                                                                                                  };
                                                                                                                                  _store.Save(entry);
                                                                                                                              });

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default) => Task.Run(() =>
                                                                                                                                                                 {
                                                                                                                                                                     var entry = _store.Get(context.FireInstanceId);
                                                                                                                                                                     if (entry != null)
                                                                                                                                                                     {
                                                                                                                                                                         entry.FinishedTimeUtc = DateTime.UtcNow;
                                                                                                                                                                         entry.ExceptionMessage = jobException?.GetBaseException()?.Message;
                                                                                                                                                                         _store.Save(entry);
                                                                                                                                                                     }

                                                                                                                                                                     if (jobException == null)
                                                                                                                                                                     {
                                                                                                                                                                         _store.IncrTotalExecuted();
                                                                                                                                                                     }
                                                                                                                                                                     else
                                                                                                                                                                     {
                                                                                                                                                                         _store.IncrTotalFailed();
                                                                                                                                                                     }
                                                                                                                                                                 });
        #endregion

        #region SchedulerPlugin
        public Task Initialize(string pluginName, IScheduler scheduler, CancellationToken cancellationToken = default) => Task.Run(() =>
                                                                                                                                    {
                                                                                                                                        Name = pluginName;
                                                                                                                                        _scheduler = scheduler;
                                                                                                                                        _scheduler.ListenerManager.AddJobListener(this, EverythingMatcher<JobKey>.AllJobs());
                                                                                                                                    });
        public Task Shutdown(CancellationToken cancellationToken = default) => Task.FromResult(0);

        public Task Start(CancellationToken cancellationToken = default) => Task.Run(() =>
                                                                                       {
                                                                                           _store = _scheduler.Context.Get(typeof(IHistoryStore).FullName) as IHistoryStore;

                                                                                           if (_store == null)
                                                                                           {
                                                                                               if (StoreType != null)
                                                                                               {
                                                                                                   _store = GlobalContext.TryResolve(StoreType) as IHistoryStore;
                                                                                               }

                                                                                               if (_store == null)
                                                                                               {
                                                                                                   throw new Exception(nameof(StoreType) + " is not set.");
                                                                                               }

                                                                                               _scheduler.Context.Put(typeof(IHistoryStore).FullName, _store);
                                                                                           }
                                                                                           _store.SchedulerName = _scheduler.SchedulerName;
                                                                                           _store.Purge();
                                                                                       });
        #endregion
    }
}
