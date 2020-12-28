using CPC.EventBus;
using System;
using System.Linq;

namespace CPC.Logger
{
    public class RabbitMessagerAsync : IMessager
    {
        private readonly MsgTemplates _templates;

        public string Name { get; private set; }
        private readonly Action<MsgTemplates> _pros;


        public RabbitMessagerAsync(MsgTemplates templates, string name = "", Action<MsgTemplates> process = null)
        {
            Name = name;
            _templates = templates;
            _templates.RabbitSetting.FileType = "MESSAGE";
            _pros = process;
            RabbitLoggerPool<IntegrationEvent>.Initialize();
        }

        public RabbitMessagerAsync(string fileName = "", string name = "", Action<MsgTemplates> process = null)
        {

            Name = name;
            var producerLogRule = RabbitLoggerConfig.GetLogRule<MessageEntity>(ConfigType.AsynMessage, fileName);

            _templates = producerLogRule.Templates.Where(t => t.Name == name).FirstOrDefault();
            if (_templates.IsNull())
            {
                _templates = producerLogRule.Templates.Where(t => t.Name == "*").FirstOrDefault();
                _templates.RabbitSetting.FileType = "MESSAGE";
            }
            _pros = process;
            RabbitLoggerPool<IntegrationEvent>.Initialize();
        }

        public IMessager Clone(string name) => new RabbitMessagerAsync(_templates, Name, _pros);
        public void Write<T>(T message) where T : IntegrationEvent
        {
            var msg = new MsgTemplates();
            if (message is MsgEntity entity)
            {
                msg.Layout = entity;
                entity.AppId = string.IsNullOrWhiteSpace(entity.AppId) ? _templates.Layout.AppId : entity.AppId;
                entity.SubAppId = string.IsNullOrWhiteSpace(entity.SubAppId) ? _templates.Layout.SubAppId : entity.SubAppId;
                entity.Extend = entity.Extend.IsNull() ? _templates.Layout.Extend : entity.Extend;
            }
            _pros?.Invoke(msg);
            RabbitLoggerPool<IntegrationEvent>.Write(_templates.RabbitSetting, message); 
        }

        /// <summary>
        /// 保留有效，不推荐使用，后续将删除，推荐使用Write方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        [Obsolete("不推荐使用,后续将删除")]
        public void WriteMsg<T>(T message) where T : MsgEntity
        {
            var msg = new MsgTemplates()
            {
                // Name = _templates.Name,
                //RabbitSetting = _templates.RabbitSetting,
                Layout = message
            };

            message.AppId = string.IsNullOrWhiteSpace(message.AppId) ? _templates.Layout.AppId : message.AppId;
            message.SubAppId = string.IsNullOrWhiteSpace(message.SubAppId) ? _templates.Layout.SubAppId : message.SubAppId;
            message.Extend = message.Extend.IsNull() ? _templates.Layout.Extend : message.Extend;
            _pros?.Invoke(msg);
            RabbitLoggerPool<IntegrationEvent>.Write(_templates.RabbitSetting, message);
           // Write(message);
        }



    }
}
