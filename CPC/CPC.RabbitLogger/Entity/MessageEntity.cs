using System;
using System.Collections.Generic;

namespace CPC.Logger
{
    [Serializable]
    public class MessageEntity
    {
        public string FileType { get; set; } = "MESSAGE";
        public List<MsgTemplates> Templates { get; set; }
    }

    public class MsgTemplates : ITemplate<MsgEntity>
    {
        public string Name { get; set; }

        public MsgEntity Layout { get; set; }
        public RabbitExternal RabbitSetting { get; set; }
    }
}
