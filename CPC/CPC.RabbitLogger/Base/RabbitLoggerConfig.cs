using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;

namespace CPC.Logger
{
    internal enum ConfigType
    {
        SyncLog,
        AsynLog,
        SyncMessage,
        AsynMessage
    }

    internal class RabbitLoggerConfig
    {
        private static readonly ConcurrentDictionary<ConfigType, string> _configs = new ConcurrentDictionary<ConfigType, string>();
        static RabbitLoggerConfig()
        {
            _configs.TryAdd(ConfigType.SyncLog, "logrule.json");
            _configs.TryAdd(ConfigType.AsynLog, "logasynrule.json");
            _configs.TryAdd(ConfigType.SyncMessage, "msgtemplate.json");
            _configs.TryAdd(ConfigType.AsynMessage, "msgasyntemplate.json");

        }

        internal static TConfig GetLogRule<TConfig>(ConfigType config, string filename = "")
        {
            if (string.IsNullOrEmpty(filename))
            {
                _configs.TryGetValue(config, out filename);
            }
            return GetLogRule<TConfig>(filename);
        }

        /// <summary>
        /// 默认取BaseDirectory下面的config，如果没有则尝试+bin
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static TConfig GetLogRule<TConfig>(string fileName)
        {
            TConfig config;
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var path1 = Path.Combine(path, fileName);
            if (File.Exists(path1))
            {
                path = path1;
            }
            else
            {
                if (path.Contains("bin"))
                {
                    path = Path.Combine(path, fileName);
                }
                else
                {
                    path = Path.Combine(path, "bin", fileName);
                }
            }

            var text = System.IO.File.ReadAllText(path, Encoding.UTF8);
            config = JsonHelper.Deserialize<TConfig>(text);

            return config;
        }

        internal static bool IgnoreLog(LogLevel level, LogLevel minLevel)
        {
            // LogLevel min = (LogLevel)Enum.Parse(typeof(LogLevel), minLevel);
            if (level < minLevel)
            {
                return true;
            }
            return false;
        }


    }
}
