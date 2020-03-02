using System;
using System.Collections.Generic;

namespace CodeMonkeys.Core.Messaging
{
    public interface ISubscriptionManager : IDisposable
    {
        IEnumerable<ISubscriberOf<TEvent>> GetSubscribersOf<TEvent>()
            where TEvent : class, IEvent;

        void Add(Type eventType, ISubscriber subscriber);

        void Remove(Type eventType, ISubscriber subscriber);
    }
}
