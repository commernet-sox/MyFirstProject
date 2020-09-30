using System;
using System.Collections.Generic;
using System.Threading;

namespace SDT.BaseTool
{
    /// <summary>
    /// 死循环检测
    /// </summary>
    public class DeadLoopCheck
    {
        #region Members
        public readonly int MaxFails;
        public int _holdErrCount = 0;
        public int HoldErrorCount => _holdErrCount;
        public List<string> ErrorKey { get; private set; } = new List<string>();
        #endregion

        #region Constructors
        public DeadLoopCheck(int maxFails = 15) => MaxFails = maxFails;
        #endregion

        #region Methods
        public void AddError(string key)
        {
            var fails = Interlocked.Increment(ref _holdErrCount);
            ErrorKey.Add(key);

            if (fails > MaxFails)
            {
                var mes = ErrorKey.JoinStr("->");
                Reset();
                throw new DeadLoopException($"严重错误: 可能死循环引用，请联系开发人员解决。最后配置链条: {mes}");
            }
        }

        public void Reset()
        {
            Interlocked.Exchange(ref _holdErrCount, 0);
            ErrorKey.Clear();
        }
        #endregion
    }

    public class DeadLoopException : Exception
    {
        public DeadLoopException(string message)
        : base(message)
        { }
    }
}
