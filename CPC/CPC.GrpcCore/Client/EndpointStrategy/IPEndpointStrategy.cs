﻿using Grpc.Core;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace CPC.GrpcCore
{
    /// <summary>
    /// IPEndPoint
    /// </summary>
    internal class IPEndpointStrategy : IEndpointStrategy
    {
        #region Members
        private readonly object _lock = new object();
        private readonly Timer _timer;
        private readonly ConcurrentDictionary<string, IEndpointDiscovery> _discoveries = new ConcurrentDictionary<string, IEndpointDiscovery>();
        private readonly ConcurrentDictionary<string, List<ServerCallInvoker>> _invokers = new ConcurrentDictionary<string, List<ServerCallInvoker>>();
        private readonly ConcurrentDictionary<string, Channel> _channels = new ConcurrentDictionary<string, Channel>();
        private readonly ChannelCredentials _credentials = ChannelCredentials.Insecure;

        private static readonly object _instanceLocker = new object();
        private static IPEndpointStrategy _ipEndpintStrategy;

        public static IPEndpointStrategy Instance
        {
            get
            {
                if (_ipEndpintStrategy != null)
                {
                    return _ipEndpintStrategy;
                }

                lock (_instanceLocker)
                {
                    if (_ipEndpintStrategy != null)
                    {
                        return _ipEndpintStrategy;
                    }

                    _ipEndpintStrategy = new IPEndpointStrategy();
                    return _ipEndpintStrategy;
                }
            }
        }
        #endregion

        #region Constructors
        internal IPEndpointStrategy()
        {
            _timer = new Timer(ClientTimespan.ResetInterval.TotalSeconds * 1000);
            InitCheckTimer();
            if (EngineContext.Initialized)
            {
                var credentials = GlobalContext.Resolve<ChannelCredentials>();
                if (credentials != null)
                {
                    _credentials = credentials;
                }
            }
        }

        ~IPEndpointStrategy()
        {
            _timer?.Stop();
            _timer?.Dispose();

            foreach (var item in _channels)
            {
                item.Value?.ShutdownAsync();
            }
            _channels.Clear();
            _invokers.Clear();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 添加ServiceDiscovery
        /// </summary>
        /// <param name="discovery"></param>
        public void AddServiceDiscovery(IEndpointDiscovery discovery) => _discoveries.AddOrUpdate(discovery.ServiceName, discovery, (k, v) => discovery);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public ServerCallInvoker Get(string serviceName)
        {
            if (_invokers.TryGetValue(serviceName, out var callInvokers) &&
                callInvokers.Count > 0)
            {
                return ServicePollingPolicy.Random(callInvokers);
            }

            lock (_lock)
            {
                if (_invokers.TryGetValue(serviceName, out callInvokers) &&
                    callInvokers.Count > 0)
                {
                    return ServicePollingPolicy.Random(callInvokers);
                }

                callInvokers = SetCallInvokers(serviceName);
                if ((callInvokers?.Count ?? 0) <= 0 && ServiceBlackPolicy.Exist(serviceName))
                {
                    callInvokers = SetCallInvokers(serviceName, false);
                }

                return ServicePollingPolicy.Random(callInvokers);
            }
        }

        /// <summary>
        /// 屏蔽
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="failedCallInvoker"></param>
        public void Revoke(string serviceName, ServerCallInvoker failedCallInvoker)
        {
            lock (_lock)
            {
                // invokers
                var failedChannel = failedCallInvoker.Channel;
                if (!_invokers.TryGetValue(serviceName, out var callInvokers) ||
                    callInvokers.All(x => !ReferenceEquals(failedChannel, x.Channel)))
                {
                    return;
                }

                callInvokers.RemoveAt(callInvokers.FindIndex(x => ReferenceEquals(failedChannel, x.Channel)));
                _invokers.AddOrUpdate(serviceName, callInvokers, (key, value) => callInvokers);

                // channels
                if (_channels.TryGetValue(failedChannel.Target, out var channel) &&
                    ReferenceEquals(channel, failedChannel))
                {
                    _channels.TryRemove(failedChannel.Target, out failedChannel);
                }

                // add black
                ServiceBlackPolicy.Add(serviceName, failedChannel.Target);
                failedChannel.ShutdownAsync();

                // if not exist invoker， call init method
                if (callInvokers.Count <= 0)
                {
                    SetCallInvokers(serviceName, false);
                }
            }
        }

        /// <summary>
        /// 初始化检查Timer
        /// </summary>
        public void InitCheckTimer()
        {
            _timer.Elapsed += (sender, e) =>
            {
                lock (_lock)
                {
                    _timer.Stop();
                    foreach (var item in _invokers)
                    {
                        SetCallInvokers(item.Key);
                    }
                    _timer.Start();
                }
            };
            _timer.Start();
        }

        /// <summary>
        /// SetCallInvokers
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="filterBlack">过滤黑名单 default true</param>
        /// <returns></returns>
        private List<ServerCallInvoker> SetCallInvokers(string serviceName, bool filterBlack = true)
        {
            if (!_discoveries.TryGetValue(serviceName, out var discovery))
            {
                return null;
            }

            _invokers.TryGetValue(serviceName, out var callInvokers);
            callInvokers = callInvokers ?? new List<ServerCallInvoker>();

            var targets = discovery.FindServiceEndpoints(filterBlack);
            var channelOptions = new List<ChannelOption>()
            {
                new ChannelOption(ChannelOptions.MaxReceiveMessageLength, int.MaxValue),
                new ChannelOption(ChannelOptions.MaxSendMessageLength, int.MaxValue),
            };
            foreach (var target in targets)
            {
                if (!_channels.TryGetValue(target, out var channel))
                {
                    channel = new Channel(target, _credentials, channelOptions);
                    _channels.AddOrUpdate(target, channel, (key, value) => channel);
                }
                if (callInvokers.Any(x => ReferenceEquals(x.Channel, channel)))
                {
                    continue;
                }

                var callInvoker = new ServerCallInvoker(channel);
                callInvokers.Add(callInvoker);
            }

            // 移除已经销毁的callInvokers
            var destroyInvokers = callInvokers.Where(oo => !targets.Contains(oo.Channel.Target)).ToList();
            foreach (var invoker in destroyInvokers)
            {
                _channels.TryRemove(invoker.Channel.Target, out var channel);
                callInvokers.Remove(invoker);
            }

            _invokers.AddOrUpdate(serviceName, callInvokers, (key, value) => callInvokers);
            return callInvokers;
        }
        #endregion
    }
}