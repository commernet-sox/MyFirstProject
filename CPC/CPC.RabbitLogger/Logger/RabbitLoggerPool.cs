using CPC.EventBus;
using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CPC.Logger
{
    public class RabbitLoggerPool<T> where T : IntegrationEvent
    {
        private static readonly ConcurrentDictionary<string, RabbitLoggerIndex<T>> _indexPools = new ConcurrentDictionary<string, RabbitLoggerIndex<T>>();
        private static readonly BlockingCollection<Tuple<T, RabbitExternal>> _mcPools = new BlockingCollection<Tuple<T, RabbitExternal>>();
        public static Action<RabbitExternal, T> Process { get; set; }
        private static bool _initStatus;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Initialize()
        {
            if (!_initStatus)
            {
                AllocationAsync();
                _initStatus = true;
            }
        }
        public static void Write(RabbitExternal external, T dILogEntity)
        {
            Process?.Invoke(external, dILogEntity);
            _mcPools.Add(new Tuple<T, RabbitExternal>(dILogEntity, external));
        }

        private static void AllocationAsync() => Task.Factory.StartNew(() =>
        {
            while (!_mcPools.IsCompleted)
            {
                var item = _mcPools.Take();
                var index = CallIndex(item.Item2);
                index.Add(item.Item1);
            }
        }, TaskCreationOptions.LongRunning);

        public static void Dispose()
        {
            //if (_producer != null)
            //{
            //    _mcPools.CompleteAdding();
            //}
        }

        private static RabbitLoggerIndex<T> CallIndex(RabbitExternal external)
        {

            var key = $"{external.Host}{external.Port}{external.Virtual}{external.Exchangename}";
            if (!_indexPools.Keys.Contains(key))
            {
                var producer = new RabbitLogProducerContent<RabbitEntity<T>>(external);
                var logIndex = new RabbitLoggerIndex<T>(producer, 500);
                _indexPools.TryAdd(key, logIndex);
                return logIndex;
            }
            else
            {

                _indexPools.TryGetValue(key, out var index);
                return index;
            }

        }
    }
}
