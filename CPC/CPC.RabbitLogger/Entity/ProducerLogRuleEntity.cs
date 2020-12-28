using System;
using System.Collections.Generic;

namespace CPC.Logger
{
    [Serializable]
    public class ProducerLogRuleEntity
    {
        public string FileType { get; set; } = "LOG";
        public List<Templates> Templates { get; set; }
        public List<LogRules> Rules { get; set; }
    }

    [Serializable]
    public class Templates : ITemplate<LoggerEntity>
    {
        public string Name { get; set; }

        public LoggerEntity Layout { get; set; }
        public RabbitExternal RabbitSetting { get; set; }
    }

    [Serializable]
    public class Layout
    {
        public string AppId { get; set; }
        public string SubAppId { get; set; }
        public string Extend { get; set; }
    }

    [Serializable]
    public class LogRules
    {
        public string Name { get; set; }
        public string Minlevel { get; set; }
        public string Template { get; set; }
    }
}
