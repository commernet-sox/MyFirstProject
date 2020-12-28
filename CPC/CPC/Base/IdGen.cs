using System;
using System.Diagnostics;
using System.Threading;

namespace CPC
{
    /// <summary>
    /// 雪花算法
    /// </summary>
    public class IdGen
    {
        #region Members
        private readonly long _machineId = 0;
        private readonly byte _machineIdBits = 0;
        private readonly byte _sequenceBits = 0;
        private readonly long _maxSequence = 0;

        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        private readonly object _lockObject = new object();

        private long _sequence = 0;
        private long _lastTimestamp = 0;

        private static readonly long _offsetTicks =
    DateTime.UtcNow.Ticks - new DateTime(2019, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
        private static IdGen _instance;
        #endregion

        #region Constructors
        /// <summary>
        /// the constructor of <see cref="IdGen"/>.
        /// </summary>
        /// <param name="machineId">当前机器码</param>
        /// <param name="machineIdBits">机器码位数（0-10之间）</param>
        /// <param name="sequenceBits">
        /// 序列号位数（0-20之间）
        /// 注意：
        /// 1. 并发量越大，此值也要越大，例如：10 可以 1 秒内生成 2^10=1024 个 ID。
        /// 2. 每台机器此参数务必相同。
        /// </param>
        public IdGen(byte machineId = 0, byte machineIdBits = 0, byte sequenceBits = 10)
        {
            if (sequenceBits > 20)
            {
                throw new ArgumentOutOfRangeException(nameof(sequenceBits), "序列号不能超过 20 位。");
            }

            if (machineIdBits > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(machineIdBits), "机器码不能超过 10 位。");
            }

            _machineIdBits = machineIdBits;
            _sequenceBits = sequenceBits;
            _maxSequence = GetMaxOfBits(_sequenceBits);

            if (machineId > 0)
            {
                var maxMachineId = GetMaxOfBits(machineId);
                if (machineId > maxMachineId)
                {
                    throw new ArgumentOutOfRangeException(nameof(machineId), $"机器码不能大于 {maxMachineId}。");
                }

                _machineId = machineId;
            }
        }

        // 获取指定长度二进制的最大整型数。例如：5 返回 000..011111。
        private static long GetMaxOfBits(byte bits) => (1L << bits) - 1; // 或 -1 ^ -1 << bits
        #endregion

        #region Methods
        private long GetTimestampNow() =>
            // 10000000 = TimeSpan.FromSeconds(1).Ticks
            (_offsetTicks + _stopwatch.Elapsed.Ticks) / 10000000L;

        private long GetNextTimestamp()
        {
            var timestamp = GetTimestampNow();
            if (timestamp < _lastTimestamp)
            {
                throw new Exception("新的时间戳比旧的小，请检查系统时间。");
            }

            while (timestamp == _lastTimestamp)
            {
                if (_sequence < _maxSequence)
                {
                    _sequence++;
                    return timestamp;
                }
                Thread.Sleep(0); // 降低CPU消耗
                timestamp = GetTimestampNow();
            }
            _sequence = 0;

            return timestamp;
        }

        /// <summary>
        /// 生成新的ID
        /// </summary>
        /// <returns>ID</returns>
        public long NewSequenceId()
        {
            lock (_lockObject)
            {
                _lastTimestamp = GetNextTimestamp();

                //int bitsLength = GetBitsLength(_lastTimestamp);
                //Console.WriteLine($"Timestamp bits: {bitsLength}");

                var timestampShift = _machineIdBits + _sequenceBits;
                int machineIdShift = _sequenceBits;
                return (_lastTimestamp << timestampShift) | (_machineId << machineIdShift) | _sequence;
            }
        }

        #endregion

        #region Static Methods
        /// <summary>
        /// Set the default instance of <see cref="IdGen"/>.
        /// </summary>
        /// <param name="idGen">The instace of <see cref="IdGen"/>.</param>
        public static void SetDefault(IdGen idGen)
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("Can't set default instance of IdGen twice.");
            }

            _instance = idGen ?? new IdGen();
        }

        public static void SetDefault(byte machineId = 0, byte machineIdBits = 0, byte sequenceBits = 10) => SetDefault(new IdGen(machineId, machineIdBits, sequenceBits));

        public static long NewId()
        {
            if (_instance == null)
            {
                throw new Exception("The IdGen has no default instance.");
            }

            return _instance.NewSequenceId();
        }

        #endregion
    }
}
