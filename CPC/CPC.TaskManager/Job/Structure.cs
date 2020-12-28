using Quartz;
using System.Collections.Generic;

namespace CPC.TaskManager
{
    public class Structure
    {
        #region Members
        public string Name { get; set; }

        public string Group { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Map Data
        /// </summary>
        public IDictionary<string, object> DataMap { get; set; } = new Dictionary<string, object>();
        #endregion

        #region Constructors
        public Structure(string name, string group = SchedulerConstants.DefaultGroup, string description = null)
        {
            Name = name;
            Group = group;
            Description = description;
        }
        #endregion

        #region Methods
        private void Initialize()
        {
            if (Name.IsNull())
            {
                Name = RandomUtility.GuidString();
            }

            if (Group.IsNull())
            {
                Group = SchedulerConstants.DefaultGroup;
            }
        }

        public virtual JobKey ToJobKey()
        {
            Initialize();
            return new JobKey(Name, Group);
        }

        public virtual TriggerKey ToTriggerKey()
        {
            Initialize();
            return new TriggerKey(Name, Group);
        }

        public virtual JobDataMap ToDataMap()
        {
            var map = new JobDataMap();
            if (!DataMap.IsNull())
            {
                map.PutAll(DataMap);
            }
            return map;
        }
        #endregion
    }
}
