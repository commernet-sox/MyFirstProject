using Polly;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CPC.Logger
{
    internal class EsIndex
    {
        #region Members
        internal readonly string Index;

        internal readonly string Type;

        private readonly int _maxBodies = 10;

        private readonly int _retryInterval = 500;

        private readonly int _maxFails = 2;

        private readonly ConcurrentQueue<EsBody> _bodies = new ConcurrentQueue<EsBody>();

        private readonly EsClient _esClient;

        private readonly CancellationTokenSource _cts;
        #endregion

        #region Constructors
        internal EsIndex(EsClient client, string index, string type, int maxBodies = 30, int retryInterval = 200, int maxFails = 3)
        {
            _esClient = client;
            Index = index;
            Type = type;
            _maxBodies = maxBodies;
            _maxFails = maxFails;
            _retryInterval = retryInterval;
            _cts = new CancellationTokenSource();
            AutoStartAsync();
        }
        #endregion

        #region Methods
        internal void Add(EsBody body) => _bodies.Enqueue(body);

        private void AutoStartAsync() => Task.Factory.StartNew(() =>
        {
            var list = new List<EsBody>();
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
                        _cts.Token.ThrowIfCancellationRequested();

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
                    var retryPolicy = Policy.Handle<Exception>().WaitAndRetry(_maxFails, _ => TimeSpan.FromMilliseconds(_retryInterval));

                    retryPolicy.Execute(() =>
                    {
                        if (!_esClient.SendBatch(Index, Type, list))
                        {
                            throw new Exception("send batch fail");
                        }
                        else if (list.Count < _maxBodies)
                        {
                            Thread.Sleep(_retryInterval);
                        }
                    });
                }
                catch
                {
                }

                list.Clear();
            }
        }, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);

        internal void Abort() => _cts.Cancel();
        #endregion
    }
}
