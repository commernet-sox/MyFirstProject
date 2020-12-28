using System.Collections.Generic;

namespace CPC.TaskManager.Plugins
{
    public interface IHistoryStore
    {
        string SchedulerName { get; set; }

        int GetTotalExecuted();

        int GetTotalFailed();

        void IncrTotalExecuted();

        void IncrTotalFailed();

        IEnumerable<History> GetLastOfEveryJob(int limit);

        IEnumerable<History> GetLastOfEveryTrigger(int limit);

        IEnumerable<History> GetLast(int limit);

        void Save(History history);

        void Purge();

        History Get(string fireInstanceId);
    }
}
