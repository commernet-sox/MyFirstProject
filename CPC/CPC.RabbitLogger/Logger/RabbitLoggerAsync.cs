using System;
using System.Linq;

namespace CPC.Logger
{
    public class RabbitLoggerAsync : ILogger
    {
        //异步写日志
        private readonly Templates _templates;
        private readonly LogLevel _minLevel;

        public string Name { get; private set; }
        private readonly Action<Templates> _pros;


        public RabbitLoggerAsync(Templates templates, LogLevel minLevel, string name = "", Action<Templates> process = null)
        {
            Name = name;
            _templates = templates.GetTempInfo();
            _templates.RabbitSetting.FileType = "LOG";
            _pros = process;
            _minLevel = minLevel;
            RabbitLoggerPool<LoggerEntity>.Initialize();
        }

        public RabbitLoggerAsync(string fileName = "", string name = "", Action<Templates> process = null)
        {

            Name = name;
            var producerLogRule = RabbitLoggerConfig.GetLogRule<ProducerLogRuleEntity>(ConfigType.AsynLog, fileName);
            var rule = producerLogRule.Rules.Where(t => t.Name == name).FirstOrDefault();
            if (rule.IsNull())
            {
                rule = producerLogRule.Rules.Where(t => t.Name == "*").FirstOrDefault();
            }
            if (rule != null)
            {
                _templates = producerLogRule.Templates.Where(t => t.Name == rule.Template).FirstOrDefault();
                _templates.GetTempInfo().RabbitSetting.FileType = "LOG";
                _minLevel = (LogLevel)Enum.Parse(typeof(LogLevel), rule.Minlevel);
            }
            _pros = process;
            RabbitLoggerPool<LoggerEntity>.Initialize();
        }

        public ILogger Clone(string name) => new RabbitLoggerAsync(_templates, _minLevel, Name, _pros);

        public void Write(LogLevel level, Exception ex, string message, params object[] args)
        {
            if (RabbitLoggerConfig.IgnoreLog(level, _minLevel)) { return; }
            var es = _templates.InitLoggerInfo(level, ex, message, args);
            _pros?.Invoke(es);
            RabbitLoggerPool<LoggerEntity>.Write(_templates.RabbitSetting, es.Layout);
        }
    }



}
