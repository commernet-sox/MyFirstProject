using CPC.EventBus;
using Newtonsoft.Json.Linq;
using System;

namespace CPC.Logger
{
    [Serializable]
    public class MsgEntity : IntegrationEvent
    {
        //报文
        public string AppId { get; set; } = string.Empty;
        public string SubAppId { get; set; } = string.Empty;
        //public string InterfaceType { get; set; } //服务代码
        //public string ServiceType { get; set; }  //服务类型
        public string ServiceName { get; set; }//接口名称
        public string EdiType { get; set; }//接口名称
        public string ReceiveBody { get; set; }
        public string ReceiveHeaders { get; set; }
        public string ReceiveUrl { get; set; }
        public string SendBody { get; set; }
        public string SendUrl { get; set; }
        public string SendHeaders { get; set; }
        public JObject Extend { get; set; }
        public bool FinishFlag { get; set; } = false; //用于处理mq 报文批量写
        public OrderParams OrderParams { get; set; } = new OrderParams();
        public MsgParams LogParams { get; set; } = new MsgParams();
    }


    [Serializable]
    public class OrderParams
    {
        public string WorkCenter { get; set; }
        public string StorerId { get; set; }
        public string WarehouseId { get; set; }
        public string BillId { get; set; }
        public string SyncBillId { get; set; }

    }
    [Serializable]
    public class MsgParams
    {
        public string Index { get; set; }
        public string Type { get; set; }

        public string KeyId { get; set; }

        public string FileName { get; set; }


    }
}
