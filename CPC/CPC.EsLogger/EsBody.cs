using System;
using System.Collections.Generic;

namespace CPC.Logger
{
    public class EsBody
    {
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime Time { get; set; } = DateTimeUtility.Now;

        /// <summary>
        /// 日志主体（相同索引的结构必须相同，否则会记录失败）
        /// </summary>
        public Dictionary<string, object> Document { get; set; } = new Dictionary<string, object>();
    }
}
