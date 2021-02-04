﻿using System;
using System.Collections.Generic;

namespace CPC.EventBus
{
    public interface IEventBusSubscriptionsManager
    {
        bool IsEmpty { get; }

        event EventHandler<string> OnEventRemoved;

        void AddSubscription<T, TH>()
           where T : IntegrationEvent
           where TH : IIntegrationEventHandler;

        void RemoveSubscription<T, TH>()
             where TH : IIntegrationEventHandler
             where T : IntegrationEvent;

        bool HasSubscriptionsForEvent<T>()
            where T : IntegrationEvent;

        bool HasSubscriptionsForEvent(string eventName);

        Type GetEventTypeByName(string eventName);

        void Clear();

        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>()
            where T : IntegrationEvent;

        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

        string GetEventKey<T>();

        string GetEventKey(Type type);

        bool CheckEventHandler<TH>()
            where TH : IIntegrationEventHandler;
    }
}