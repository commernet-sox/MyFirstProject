using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CPC.TaskManager.Plugins
{
    public class HistoryStore : IHistoryStore
    {
        #region Members
        private readonly HashSet<History> _histories = new HashSet<History>();

        private static int _maxSize = 1000;
        private static double _scale = 0.9d;

        private int _totalJobsExecuted = 0;
        private int _totalJobsFailed = 0;

        public string SchedulerName { get; set; }
        #endregion

        #region Methods
        public static void SetOptions(int maxSize, double scale)
        {
            _maxSize = maxSize;
            if (scale > 1 || scale <= 0)
            {
                throw new ArgumentException("scale error");
            }
            _scale = scale;
        }

        public virtual History Get(string fireInstanceId)
        {
            lock (_histories)
            {
                return _histories.FirstOrDefault(t => t.FireInstanceId == fireInstanceId);
            }
        }

        public virtual IEnumerable<History> GetLast(int limit)
        {
            lock (_histories)
            {
                return _histories.Where(t => t.SchedulerName == SchedulerName).OrderByDescending(t => t.ActualFireTimeUtc).Take(limit).ToArray();
            }
        }

        public virtual IEnumerable<History> GetLastOfEveryJob(int limit)
        {
            lock (_histories)
            {
                return _histories.Where(t => t.SchedulerName == SchedulerName).GroupBy(t => t.Job).Select(t => t.OrderByDescending(o => o.ActualFireTimeUtc).Take(limit)).SelectMany(t => t).ToArray();
            }
        }

        public virtual IEnumerable<History> GetLastOfEveryTrigger(int limit)
        {
            lock (_histories)
            {
                return _histories.Where(t => t.SchedulerName == SchedulerName).GroupBy(t => t.Trigger).Select(t => t.OrderByDescending(o => o.ActualFireTimeUtc).Take(limit)).SelectMany(t => t).ToArray();
            }
        }

        public virtual int GetTotalExecuted() => _totalJobsExecuted;

        public virtual int GetTotalFailed() => _totalJobsFailed;

        public virtual void IncrTotalExecuted() => Interlocked.Increment(ref _totalJobsExecuted);

        public virtual void IncrTotalFailed() => Interlocked.Increment(ref _totalJobsFailed);

        public virtual void Purge()
        {
            lock (_histories)
            {
                if (_histories.Count > _maxSize)
                {
                    var sch = _histories.Where(t => t.SchedulerName == SchedulerName);
                    var stas = sch.GroupBy(t => t.Trigger).Select(t => new { Count = t.Count(), Trigger = t.Key }).ToArray();
                    foreach (var item in stas)
                    {
                        var count = Math.Round(item.Count * (1 - _scale)).ConvertInt32();
                        if (count > 0)
                        {
                            var list = sch.Where(t => t.Trigger == item.Trigger).OrderBy(t => t.ActualFireTimeUtc).Take(count).ToList();
                            list.ForEach(t => _histories.Remove(t));
                            list.Clear();
                        }
                    }
                }
            }
        }

        public virtual void Save(History history)
        {
            Purge();

            lock (_histories)
            {
                var data = _histories.FirstOrDefault(t => t.FireInstanceId == history.FireInstanceId);
                if (data != null)
                {
                    _histories.Remove(data);
                }
                _histories.Add(history);
            }
        }
        #endregion
    }
}
