using System;
using System.Collections.Generic;
using System.Text;

namespace SDT.BaseTool
{
    /// <summary>
    /// 由启动时运行的任务实现的接口
    /// </summary>
    public interface IEngineService
    {
        /// <summary>
        /// start a service
        /// </summary>
        void Start();

        void Stop();
    }
}
