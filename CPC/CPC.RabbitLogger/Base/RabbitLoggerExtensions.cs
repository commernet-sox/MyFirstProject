using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CPC.Logger
{
    public static class RabbitLoggerExtensions
    {
        public static string GetExceptionInfo(this Exception e)
        {
            var exInfo = new StringBuilder();
            exInfo.AppendLine($" An exception ({e.GetType().Name}) occurred. ");
            exInfo.AppendLine($" Message:{ e.Message} ");
            exInfo.AppendLine($" TargetSite:{ e.TargetSite} ");
            exInfo.AppendLine($" Source:{ e.Source} ");
            exInfo.AppendLine($" StackTrace:{ e.StackTrace} ");
            var ie = e.InnerException;
            if (ie != null)
            {
                exInfo.AppendLine($" The Inner Exception:");
                exInfo.AppendLine($" Exception Name: {ie.GetType().Name}");
                exInfo.AppendLine($" Message: {ie.Message}");
                exInfo.AppendLine($" Stack Trace:   {ie.StackTrace}");
                exInfo.AppendLine($" TargetSite:{ e.TargetSite} ");
                exInfo.AppendLine($" Source:{ e.Source} ");
            }
            return exInfo.ToString();
        }


        public static Templates InitLoggerInfo(this Templates tmp, LogLevel level, Exception ex, string message, params object[] args)
        {
            var templates = new Templates
            {
                Name = tmp.Name,
                RabbitSetting = tmp.RabbitSetting,
                Layout = new LoggerEntity()
            };
            templates.Layout.AppId = tmp.Layout.AppId;
            templates.Layout.SubAppId = tmp.Layout.SubAppId;
            templates.Layout.Extend = tmp.Layout.Extend;
            templates.Layout.ProcessId = tmp.Layout.ProcessId;
            templates.Layout.ProcessName = tmp.Layout.ProcessName;

            var logEntity = templates.Layout;
            logEntity.Message = args.IsNull() ? message : string.Format(message, args);
            StackTrace st;
            if (!ex.IsNull())
            {
                st = new StackTrace(ex, true);
                logEntity.Exception = ex.GetExceptionInfo();
                logEntity.Stacktrace = BuildStackTraceMessage(logEntity, st, true);
            }
            else
            {
                st = new StackTrace(0, true);

                logEntity.Stacktrace = BuildStackTraceMessage(logEntity, st);
            }
            logEntity.Lever = level;

            logEntity.AssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            logEntity.ProcessTime = DateTimeUtility.Now.ToString("");

            return templates;
        }

        public static Templates GetTempInfo(this Templates templates)
        {
            var processes = Process.GetCurrentProcess();
            var logEntity = templates.Layout;

            logEntity.ProcessId = processes.Id.ToString();
            logEntity.ProcessName = processes.ProcessName.ToString();
            return templates;
        }

        /// <summary>
        /// log message 用于获取模板默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="layout"></param>
        /// <param name="msgEntity"></param>
        /// <returns></returns>
        public static T GetTemplateInfo<T>(this T layout, T msgEntity)
        {
            var template = JObject.FromObject(layout);
            var enti = JObject.FromObject(msgEntity);
            foreach (JProperty item in template.Children())
            {

                if (!string.IsNullOrWhiteSpace(item.Value.ToString().Trim()) && string.IsNullOrWhiteSpace(enti[item.Name].ToString()))
                {
                    enti[item.Name] = item.Value;
                }
            }
            msgEntity = JsonHelper.Deserialize<T>(JsonHelper.Serialize(enti));
            return msgEntity;
        }

        public static string BuildStackTraceMessage(LoggerEntity loggerEntity, StackTrace stackTrace, bool isNeedReverses = false)
        {
            if (stackTrace != null)
            {
                var frameList = stackTrace.GetFrames();
                if (frameList.IsNull())
                {
                    return string.Empty;
                }

                var realFrameList = from p in frameList
                                    let ns = p?.GetMethod()?.DeclaringType?.Namespace
                                    where !ns.IsNull() && !ns.StartsWith("CPC.Logger", StringComparison.OrdinalIgnoreCase) && !ns.StartsWith("System", StringComparison.OrdinalIgnoreCase) && !ns.StartsWith("Microsoft", StringComparison.OrdinalIgnoreCase)
                                    select p;

                if (realFrameList != null && realFrameList.Any())
                {
                    var builder = new StringBuilder();
                    if (isNeedReverses)
                    {
                        realFrameList = realFrameList.Reverse();
                    }
                    var lastFrame = realFrameList.First();
                    var type = lastFrame.GetMethod().DeclaringType;
                    loggerEntity.TypeInfo = type.GetTypeName();
                    builder.AppendFormat("源文件：{0}", lastFrame.GetFileName()).AppendLine();
                    if (lastFrame.GetFileLineNumber() > 0)
                    {
                        builder.AppendFormat("行号：{0}", lastFrame.GetFileLineNumber()).AppendLine();
                    }
                    builder.AppendFormat("方法名：{0}", lastFrame.GetMethod().ToString()).AppendLine();
                    builder.AppendLine("堆栈跟踪：");
                    builder.AppendLine("=================================================================");

                    MethodBase method;
                    foreach (var frame in realFrameList)
                    {
                        method = frame.GetMethod();
                        var fileline = frame.GetFileLineNumber() > 0 ? $"第{ frame.GetFileLineNumber()}行" : "";
                        builder.AppendFormat("> {0} 类下的{1} {2} 方法", method.DeclaringType.ToString(), fileline, method.ToString()).AppendLine();
                    }
                    builder.AppendLine("=================================================================");
                    return builder.ToString();
                }
            }
            return "";
        }


    }
}
