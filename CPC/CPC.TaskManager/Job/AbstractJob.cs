using Quartz;
using System;
using System.Threading.Tasks;

namespace CPC.TaskManager
{
    [Serializable, PersistJobDataAfterExecution]
    public abstract class AbstractJob : IJob
    {
        #region Methods
        public abstract Task Execute(IJobExecutionContext context);
        #endregion
    }
}
