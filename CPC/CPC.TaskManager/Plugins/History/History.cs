using System;

namespace CPC.TaskManager.Plugins
{
    public class History
    {
        public string FireInstanceId { get; set; }
        public string SchedulerInstanceId { get; set; }
        public string SchedulerName { get; set; }
        public string Job { get; set; }
        public string Trigger { get; set; }
        public DateTime? ScheduledFireTimeUtc { get; set; }
        public DateTime ActualFireTimeUtc { get; set; }
        public bool Recovering { get; set; }
        public bool Vetoed { get; set; }
        public DateTime? FinishedTimeUtc { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
