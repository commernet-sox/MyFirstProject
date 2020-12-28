using System;
using System.Collections.Generic;
using System.Linq;

namespace CPC.Logger
{
    public class RabbitLoggerSync : ILogger
    {
        //mq 同步写日志
        private readonly Templates _templates;

        public string Name { get; private set; }
        private readonly Action<Templates> _pros;
        private readonly RabbitLogProducerContent<RabbitEntity<LoggerEntity>> _producer;
        private readonly LogLevel _minLevel;

        public RabbitLoggerSync(Templates templates, LogLevel minLevel, string name = "", Action<Templates> process = null)
        {
            Name = name;
            _minLevel = minLevel;
            _templates = templates.GetTempInfo();
            _templates.RabbitSetting.FileType = "LOG";
            _pros = process;
            _producer = new RabbitLogProducerContent<RabbitEntity<LoggerEntity>>(_templates.RabbitSetting);
        }

        public RabbitLoggerSync(string fileName = "", string name = "", Action<Templates> process = null)
        {

            Name = name;
            var producerLogRule = RabbitLoggerConfig.GetLogRule<ProducerLogRuleEntity>(ConfigType.SyncLog, fileName);
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
            _producer = new RabbitLogProducerContent<RabbitEntity<LoggerEntity>>(_templates.RabbitSetting);
        }

        public ILogger Clone(string name) => new RabbitLoggerAsync(_templates, _minLevel, Name, _pros);

        public void Write(LogLevel level, Exception ex, string message, params object[] args)
        {
            if (RabbitLoggerConfig.IgnoreLog(level, _minLevel)) { return; }
            var es = _templates.InitLoggerInfo(level, ex, message, args);
            _pros?.Invoke(es);
            _producer.Send(new RabbitEntity<LoggerEntity>() { Contexts = new List<LoggerEntity>() { es.Layout } });
        }
    }
}
