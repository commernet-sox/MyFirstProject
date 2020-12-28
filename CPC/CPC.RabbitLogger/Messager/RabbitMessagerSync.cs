using CPC.EventBus;
using System;
using System.Linq;

namespace CPC.Logger
{
    public class RabbitMessagerSync : IMessager
    {
        private readonly MsgTemplates _templates;

        public string Name { get; private set; }
        private readonly Action<MsgTemplates> _pros;

        public RabbitMessagerSync(MsgTemplates templates, string name = "", Action<MsgTemplates> process = null)
        {
            Name = name;
            _templates = templates;
            _templates.RabbitSetting.FileType = "MESSAGE";
            _pros = process;
        }

        public RabbitMessagerSync(string fileName = "", string name = "", Action<MsgTemplates> process = null)
        {

            Name = name;
            var producerLogRule = RabbitLoggerConfig.GetLogRule<MessageEntity>(ConfigType.SyncMessage, fileName);

            _templates = producerLogRule.Templates.Where(t => t.Name == name).FirstOrDefault();
            if (_templates.IsNull())
            {
                _templates = producerLogRule.Templates.Where(t => t.Name == "*").FirstOrDefault();
                _templates.RabbitSetting.FileType = "MESSAGE";
            }
            _pros = process;


        }

        public IMessager Clone(string name) => new RabbitMessagerSync(_templates, Name, _pros);

        public void Write<T>(T message) where T : IntegrationEvent
        {
            var msg = new MsgTemplates();
            if (message is RabbitEntity<MsgEntity> entity)
            {
                msg.Layout = entity.Contexts[0];
                for (var i = 0; i < entity.Contexts.Count; i++)
                {
                    entity.Contexts[i].AppId = string.IsNullOrWhiteSpace(entity.Contexts[i].AppId) ? _templates.Layout.AppId : entity.Contexts[i].AppId;
                    entity.Contexts[i].SubAppId = string.IsNullOrWhiteSpace(entity.Contexts[i].SubAppId) ? _templates.Layout.SubAppId : entity.Contexts[i].SubAppId;
                    entity.Contexts[i].Extend = entity.Contexts[i].Extend.IsNull() ? _templates.Layout.Extend : entity.Contexts[i].Extend;
                }
            }
            _pros?.Invoke(msg);
            var producer = new RabbitLogProducerContent<T>(_templates.RabbitSetting);
            producer.Send(message);
        }

        /// <summary>
        /// 保留有效，不推荐使用，推荐使用Write方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        [Obsolete("不推荐使用,后续将删除")]
        public void WriteMsg<T>(T message) where T : RabbitEntity<MsgEntity>
        {
            var msg = new MsgTemplates();
            msg.Layout = message.Contexts[0];
            for (var i = 0; i < message.Contexts.Count; i++)
            {
                message.Contexts[i].AppId = string.IsNullOrWhiteSpace(message.Contexts[i].AppId) ? _templates.Layout.AppId : message.Contexts[i].AppId;
                message.Contexts[i].SubAppId = string.IsNullOrWhiteSpace(message.Contexts[i].SubAppId) ? _templates.Layout.SubAppId : message.Contexts[i].SubAppId;
                message.Contexts[i].Extend = message.Contexts[i].Extend.IsNull() ? _templates.Layout.Extend : message.Contexts[i].Extend;
            }
            _pros?.Invoke(msg);
            var producer = new RabbitLogProducerContent<T>(_templates.RabbitSetting);
            producer.Send(message);


        }
    }
}
