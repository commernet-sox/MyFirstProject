using CPC.TaskManager.Plugins.TypeHandlers;
using Microsoft.AspNetCore.Http;
using Quartz;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CPC.TaskManager.Plugins
{
    internal static partial class Extensions
    {
        internal static TypeHandlerBase[] Order(this IEnumerable<TypeHandlerBase> typeHandlers) => typeHandlers.OrderBy(x => x.DisplayName).ToArray();

        internal static JobDataMapItemBase[] GetModel(this IEnumerable<Dictionary<string, object>> formData, Services services) => formData.Select(x => JobDataMapItemBase.FromDictionary(x, services)).Where(x => !x.IsLast).ToArray();

        internal static string ToDefaultFormat(this DateTime date) => date.ToString(DateTimeSettings.DefaultDateFormat + " " + DateTimeSettings.DefaultTimeFormat, CultureInfo.InvariantCulture);

        internal static Dictionary<string, string> ToDictionary(this IEnumerable<TimeZoneInfo> timeZoneInfos) => timeZoneInfos.ToDictionary(x => x.Id, x =>
                                                                                                                         {
                                                                                                                             var title = x.ToString();
                                                                                                                             if (!title.StartsWith("("))
                                                                                                                             {
                                                                                                                                 title = $"({title}) {x.Id}";
                                                                                                                             }

                                                                                                                             return title;
                                                                                                                         });

        internal static IEnumerable<ICalendar> Flatten(this ICalendar root)
        {
            while (root != null)
            {
                yield return root;
                root = root.CalendarBase;
            }
        }

        internal static string ETag(this DateTime dateTime)
        {
            var etagHash = dateTime.ToFileTimeUtc();
            return '\"' + Convert.ToString(etagHash, 16) + '\"';
        }

        internal static string ReadAsString(this HttpRequest request)
        {
            using (var ms = new MemoryStream())
            {
                request.Body.CopyTo(ms);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        internal static JobDataMap GetQuartzJobDataMap(this IEnumerable<JobDataMapItemBase> models)
        {
            var map = new JobDataMap();
            foreach (var item in models)
            {
                map.Put(item.Name, item.Value);
            }

            return map;
        }

        internal static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source)
        {
            using (var it = source.GetEnumerator())
            {
                if (it.MoveNext())
                {
                    var item = it.Current;
                    while (it.MoveNext())
                    {
                        yield return item;
                        item = it.Current;
                    }
                }
            }
        }

        internal static object GetValue(this IDictionary<string, object> dict, string key, object @default)
        {
            if (dict.TryGetValue(key, out var value))
            {
                return value;
            }
            else
            {
                return @default;
            }
        }

        internal static string[] GroupArray(this IEnumerable<string> seq) => seq.Concat(new[] { SchedulerConstants.DefaultGroup }).Distinct().OrderBy(x => x).ToArray();

        internal static string[] GroupArray(this IEnumerable<JobKey> seq) => seq.Select(x => x.Group).GroupArray();

        internal static List<JobDataMapItem> GetJobDataMapModel(this IJobDetail job, Services services) => GetJobDataMapModelCore(job, services);

        internal static List<JobDataMapItem> GetJobDataMapModel(this ITrigger trigger, Services services) => GetJobDataMapModelCore(trigger, services);

        private static List<JobDataMapItem> GetJobDataMapModelCore(object jobOrTrigger, Services services)
        {
            var list = new List<JobDataMapItem>();

            // TODO: doplnit parametre z template na zaklade jobKey; value najprv skonvertovat na ocakavany typ zo sablony

            JobDataMap jobDataMap = null;

            {
                if (jobOrTrigger is IJobDetail j)
                {
                    jobDataMap = j.JobDataMap;
                }

                if (jobOrTrigger is ITrigger t)
                {
                    jobDataMap = t.JobDataMap;
                }
            }

            if (jobDataMap == null)
            {
                throw new ArgumentException("Invalid type.", nameof(jobOrTrigger));
            }

            foreach (var pair in jobDataMap)
            {
                JobDataMapItem model;

                model = new JobDataMapItem()
                {
                    Enabled = true,
                    Name = pair.Key,
                    Value = pair.Value,
                };

                var typeHandlers = new List<TypeHandlerBase>();
                typeHandlers.AddRange(services.Options.StandardTypes);

                if (model.Value == null)
                {
                    model.SelectedType = services.Options.DefaultSelectedType;
                }
                else
                {
                    // find the best TypeHandler
                    foreach (var t in typeHandlers)
                    {
                        if (t.CanHandle(model.Value))
                        {
                            model.SelectedType = t;
                            break;
                        }
                    }

                    if (model.SelectedType == null) // if there is no suitable TypeHandler, create dynamic one
                    {
                        var t = model.Value.GetType();

                        string strValue;
                        var m = t.GetMethod(nameof(ToString), BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.Any, Array.Empty<Type>(), null);
                        if (m.DeclaringType == typeof(object))
                        {
                            strValue = "{" + t.ToString() + "}";
                        }
                        else
                        {
                            strValue = string.Format(CultureInfo.InvariantCulture, "{0}", model.Value);
                        }

                        model.SelectedType = new UnsupportedTypeHandler()
                        {
                            Name = Guid.NewGuid().ToString("N"), // assure unique name
                            AssemblyQualifiedName = t.GetTypeName(),
                            DisplayName = "Unsupported",
                            StringValue = strValue
                        };

                        typeHandlers.Add(model.SelectedType);
                    }
                }

                model.SupportedTypes = typeHandlers.Order();

                list.Add(model);
            }

            return list;
        }

        internal static TriggerType GetTriggerType(this ITrigger trigger)
        {
            if (trigger is ICronTrigger)
            {
                return TriggerType.Cron;
            }

            if (trigger is IDailyTimeIntervalTrigger)
            {
                return TriggerType.Daily;
            }

            if (trigger is ISimpleTrigger)
            {
                return TriggerType.Simple;
            }

            if (trigger is ICalendarIntervalTrigger)
            {
                return TriggerType.Calendar;
            }

            return TriggerType.Unknown;
        }

        internal static string GetScheduleDescription(this ITrigger trigger)
        {
            if (trigger is ICronTrigger cr)
            {
                return ExpressionDescriptor.GetDescription(cr.CronExpressionString);
            }

            if (trigger is IDailyTimeIntervalTrigger dt)
            {
                return GetScheduleDescription(dt);
            }

            if (trigger is ISimpleTrigger st)
            {
                return GetScheduleDescription(st);
            }

            if (trigger is ICalendarIntervalTrigger ct)
            {
                return GetScheduleDescription(ct.RepeatInterval, ct.RepeatIntervalUnit);
            }

            return null;
        }

        private class TimespanPart
        {
            internal static readonly TimespanPart[] Items = new[]
            {
            new TimespanPart("day", 1000 * 60 * 60 * 24),
            new TimespanPart("hour", 1000 * 60 * 60),
            new TimespanPart("minute", 1000 * 60),
            new TimespanPart("second", 1000),
            new TimespanPart("millisecond", 1),
            };

            public string Singular { get; set; }
            public string Plural { get; set; }
            public long Multiplier { get; set; }

            public TimespanPart(string singular, long multiplier) : this(singular) => Multiplier = multiplier;
            public TimespanPart(string singular)
            {
                Singular = singular;
                Plural = singular + "s";
            }
        }

        internal static string GetScheduleDescription(this IDailyTimeIntervalTrigger trigger)
        {
            var result = GetScheduleDescription(trigger.RepeatInterval, trigger.RepeatIntervalUnit, trigger.RepeatCount);
            result += " from " + trigger.StartTimeOfDay.ToShortFormat() + " to " + trigger.EndTimeOfDay.ToShortFormat();

            if (trigger.DaysOfWeek.Count < 7)
            {
                var dow = DaysOfWeekViewModel.Create(trigger.DaysOfWeek);

                if (dow.AreOnlyWeekdaysEnabled)
                {
                    result += " only on Weekdays";
                }
                else if (dow.AreOnlyWeekendEnabled)
                {
                    result += " only on Weekends";
                }
                else
                {
                    result += " on " + string.Join(", ", trigger.DaysOfWeek);
                }
            }

            return result;
        }

        internal static string GetScheduleDescription(this ISimpleTrigger trigger)
        {
            var result = "Repeat ";
            if (trigger.RepeatCount > 0)
            {
                result += trigger.RepeatCount + " times ";
            }

            result += "every ";

            var diff = trigger.RepeatInterval.TotalMilliseconds;

            var messagesParts = new List<string>();
            foreach (var part in TimespanPart.Items)
            {
                var currentPartValue = Math.Floor(diff / part.Multiplier);
                diff -= currentPartValue * part.Multiplier;

                if (currentPartValue == 1)
                {
                    messagesParts.Add(part.Singular);
                }
                else if (currentPartValue > 1)
                {
                    messagesParts.Add(currentPartValue + " " + part.Plural);
                }
            }

            result += string.Join(", ", messagesParts);

            return result;
        }

        internal static IHistoryStore GetHistoryStore(this SchedulerContext context) => context.Get(typeof(IHistoryStore).FullName) as IHistoryStore;

        internal static string GetScheduleDescription(int repeatInterval, IntervalUnit repeatIntervalUnit, int repeatCount = 0)
        {
            var result = "Repeat ";
            if (repeatCount > 0)
            {
                result += repeatCount + " times ";
            }

            result += "every ";

            var unitStr = repeatIntervalUnit.ToString().ToLower();

            if (repeatInterval == 1)
            {
                result += unitStr;
            }
            else
            {
                result += repeatInterval + " " + unitStr + "s";
            }

            return result;
        }


        internal static string ToShortFormat(this TimeOfDay timeOfDay) => timeOfDay.ToTimeSpan().ToString("g", CultureInfo.InvariantCulture);

        internal static TimeSpan ToTimeSpan(this TimeOfDay timeOfDay) => TimeSpan.FromSeconds(timeOfDay.Second + timeOfDay.Minute * 60 + timeOfDay.Hour * 3600);

        internal static TriggerBuilder ForJob(this TriggerBuilder builder, string jobKey)
        {
            var parts = jobKey.Split('.');
            return builder.ForJob(new JobKey(parts[1], parts[0]));
        }

        internal static TimeOfDay ToTimeOfDay(this TimeSpan timeSpan) => new TimeOfDay(timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

        internal static IEnumerable<TElement> TryGet<TKey, TElement>(this ILookup<TKey, TElement> lookup, TKey key) => lookup.Contains(key) ? lookup[key] : null;

        internal static Histogram ToHistogram(this IEnumerable<History> entries, bool detailed = false)
        {
            if (entries == null || entries.Any() == false)
            {
                return null;
            }

            var hst = new Histogram();
            foreach (var entry in entries)
            {
                TimeSpan? duration = null;
                var cssClass = "";
                var state = "Finished";

                if (entry.FinishedTimeUtc != null)
                {
                    duration = entry.FinishedTimeUtc - entry.ActualFireTimeUtc;
                }

                if (entry.Vetoed == false && entry.FinishedTimeUtc == null) // still running
                {
                    duration = DateTime.UtcNow - entry.ActualFireTimeUtc;
                    cssClass = "running";
                    state = "Running";
                }

                if (entry.Vetoed)
                {
                    state = "Vetoed";
                }

                string durationHtml = "", delayHtml = "", errorHtml = "", detailsHtml = "";

                if (!string.IsNullOrEmpty(entry.ExceptionMessage))
                {
                    state = "Failed";
                    cssClass = "failed";
                    errorHtml = $"<br>Error: <b>{entry.ExceptionMessage}</b>";
                }

                if (duration != null)
                {
                    durationHtml = $"<br>Duration: <b>{duration.ToNiceFormat()}</b>";
                }

                if (entry.ScheduledFireTimeUtc != null)
                {
                    delayHtml = $"<br>Delay: <b>{(entry.ActualFireTimeUtc - entry.ScheduledFireTimeUtc).ToNiceFormat()}</b>";
                }

                if (detailed)
                {
                    detailsHtml = $"Job: <b>{entry.Job}</b><br>Trigger: <b>{entry.Trigger}</b><br>";
                }

                hst.AddBar(duration?.TotalSeconds ?? 1,
                    $"{detailsHtml}Fired: <b>{entry.ActualFireTimeUtc.ToDefaultFormat()} UTC</b>{durationHtml}{delayHtml}" +
                    $"<br>State: <b>{state}</b>{errorHtml}",
                    cssClass);
            }

            return hst;
        }

        internal static string ToNiceFormat(this TimeSpan? timeSpan)
        {
            if (timeSpan == null)
            {
                return "";
            }

            var ts = timeSpan.Value;

            if (ts.TotalSeconds < 1)
            {
                return (int)ts.TotalMilliseconds + "ms";
            }

            if (ts.TotalMinutes < 1)
            {
                return (int)ts.TotalSeconds + " seconds";
            }

            if (ts.TotalHours < 1)
            {
                return (int)ts.TotalMinutes + " minutes";
            }

            if (ts.TotalDays < 1)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0:hh\\:mm}", timeSpan);
            }

            return string.Format(CultureInfo.InvariantCulture, "{0:%d} days {0:hh\\:mm}", timeSpan);
        }

    }
}
