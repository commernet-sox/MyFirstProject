using System.Collections.Generic;

namespace CPC.TaskManager.Plugins
{
    public class JobDataMapModel
    {
        public List<JobDataMapItem> Items { get; } = new List<JobDataMapItem>();
        public JobDataMapItem Template { get; set; }
    }
}
