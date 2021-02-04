using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace StudyExtend.AspectCore
{
    /// <summary>
    /// 测试AspectCore AOP代码植入
    /// </summary>
    
    public  class SampleService
    {
        /// <summary>
        /// 计数
        /// </summary>
        /// <returns></returns>
        [TimeAspect]
        public virtual int GetCount()
        {
            Thread.Sleep(3000);
            return 10;
        }
    }
}
