using CodeMonkeys.Core.Messaging;

using System;
using System.Collections.Generic;

namespace CodeMonkeys.Messaging
{
    internal interface ISubscriptionManager
    {
        IEnumerable<ISubscriberOf<TEvent>> GetSubscribersOf<TEvent>()
            where TEvent : class, IEvent;

        void Add(Type eventType, ISubscriber subscriber);

        void Remove(Type eventType, ISubscriber subscriber);
    }
}
