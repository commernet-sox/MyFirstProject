using System;

namespace CPC.TaskManager
{
    /// <summary>
    /// Job 设置
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class JobPropertyAttribute : Attribute
    {
        /// <summary>
        /// 是否自动恢复
        /// </summary>
        public bool Recovery { get; set; } = false;

        /// <summary>
        /// 此任务是否启用，默认为True
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 基本信息
        /// </summary>
        public Structure BaseInfo { get; set; }
    }
}
