using CPC.EventBus;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;

namespace CPC.Logger
{
    [Serializable]
    public class LoggerEntity : IntegrationEvent
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; } = string.Empty;

        /// <summary>
        /// 子应用ID
        /// </summary>
        public string SubAppId { get; set; } = string.Empty;

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public LogLevel Lever { get; set; }

        /// <summary>
        /// 当前进程ID
        /// </summary>
        public string ProcessId { get; set; }

        /// <summary>
        /// 当前进程名
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 线程名称
        /// </summary>
        public string ThreadName { get; set; }

        public string Stacktrace { get; set; }

        public string TypeInfo { get; set; }

        public string HostName { get; set; }

        public string LocalIp { get; set; }

        public string AssemblyVersion { get; set; }

        public string ProcessTime { get; set; }

        public JObject Extend { get; set; }

        public bool FinishFlag { get; set; }
    }
}
