using Quartz;
using System;
using System.Collections.Generic;

namespace CPC.TaskManager
{
    public class JobTriggers
    {
        internal IList<TriggerBuilder> TriggerCaches { get; } = new List<TriggerBuilder>();

        public virtual JobTriggers AddTrigger(Structure trigger = null, Action<TriggerBuilder> triggerSetup = null)
        {
            trigger = trigger ?? new Structure(RandomUtility.GuidString());

            var builder = TriggerBuilder.Create().WithIdentity(trigger.ToTriggerKey()).WithDescription(trigger.Description).UsingJobData(trigger.ToDataMap());
            triggerSetup?.Invoke(builder);
            return AddTrigger(builder);
        }

        public virtual JobTriggers AddTrigger(TriggerBuilder builder)
        {
            TriggerCaches.Add(builder);
            return this;
        }

        public JobTriggers ClearTrigger()
        {
            TriggerCaches.Clear();
            return this;
        }
    }
}
