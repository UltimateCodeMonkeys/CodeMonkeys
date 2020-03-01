using CodeMonkeys.Core.Messaging;
using CodeMonkeys.Messaging.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CodeMonkeys.Messaging
{
    internal class SubscriptionRegistry
    {
        private readonly CancellationToken _token;

        private readonly HashSet<Subscription> _items;
        private readonly object _sync;

        private readonly SubscriptionOptions _options;

        internal SubscriptionRegistry(
            SubscriptionOptions options,
            CancellationToken token)
        {
            _token = token;

            _items = new HashSet<Subscription>();
            _sync = new object();

            _options = options;

            Task.Run(RemoveCollectedSubscribers);
        }

        internal IEnumerable<ISubscriberOf<TEvent>> GetSubscribersOf<TEvent>()
            where TEvent : class, IEvent
        {
            var subscriptions = _items
                .Where(subscription => subscription.EventType.Equals(typeof(TEvent)));

            return subscriptions
                .Select(subscription => subscription.Reference.Target as ISubscriberOf<TEvent>);
        }

        internal void Add(Type eventType, ISubscriber subscriber)
        {
            lock (_sync)
            {
                _items.Add(
                    Subscription.Create(
                        eventType, 
                        subscriber));
            }
        }

        internal void Remove(Type eventType, ISubscriber subscriber)
        {
            lock (_sync)
            {
                _items.Remove(
                    Subscription.Create(
                        eventType, 
                        subscriber));
            }
        }

        private async Task RemoveCollectedSubscribers()
        {
            while (!_token.IsCancellationRequested)
            {
                foreach (var item in _items)
                {
                    if (item.Reference.Target == null)
                        Remove(item.EventType, item.Reference.Target as ISubscriber);
                }

                await Task.Delay(_options.FlushSubscriptionsPeriod);
            }
        }
    }
}
