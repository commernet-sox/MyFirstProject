using CPC.EventBus;
using Polly;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CPC.Logger
{
    public class RabbitLoggerIndex<T> where T : IntegrationEvent
    {
        #region member
        private readonly int _maxBodies = 200;

        private readonly int _retryInterval = 500;

        private readonly int _maxFails = 2;

        private readonly ConcurrentQueue<T> _bodies = new ConcurrentQueue<T>();
        private readonly CPC.Logger.ILogger dlogger;
        private readonly Policy _retryPolicy;
        private bool _isAbort = false;
        private readonly RabbitLogProducerContent<RabbitEntity<T>> _producer;
        #endregion

        public RabbitLoggerIndex(RabbitLogProducerContent<RabbitEntity<T>> producer, int maxBodies = 10, int retryInterval = 500, int maxFails = 2)
        {
            _maxBodies = maxBodies;
            _maxFails = maxFails;
            _retryInterval = retryInterval;
            _isAbort = false;
            _producer = producer;

            _retryPolicy = Policy.Handle<Exception>().WaitAndRetry(_maxFails, _ => TimeSpan.FromMilliseconds(_retryInterval));
            dlogger = new NLogger();
            AutoStartAsync();
        }

        #region Methods
        internal void Add(T body) => _bodies.Enqueue(body);

        private void AutoStartAsync() => Task.Factory.StartNew(() =>
        {
            var list = new List<T>();
            while (true)
            {
                var isSend = false;
                if (_bodies.TryDequeue(out var body))
                {
                    list.Add(body);
                    if (list.Count >= _maxBodies)
                    {
                        isSend = true;
                    }
                }
                else
                {
                    isSend = list.Count > 0;

                    if (!isSend)
                    {
                        if (_isAbort)
                        {
                            return;
                        }

                        Thread.Sleep(_retryInterval);
                        continue;
                    }
                }

                if (!isSend)
                {
                    continue;
                }

                try
                {
                    _retryPolicy.Execute(() =>
                    {
                        if (!_producer.Send(new RabbitEntity<T> { Contexts = list }))
                        {
                            dlogger.Warn($" send batch fail{JsonHelper.Serialize(list)}");
                        }
                        else if (list.Count < _maxBodies)
                        {
                            Thread.Sleep(_retryInterval);
                        }
                    });
                }
                catch (Exception ex)
                { dlogger.Error(ex); }

                list.Clear();
            }

        }, TaskCreationOptions.LongRunning);

        internal void Abort() => _isAbort = true;
        #endregion
    }
}
