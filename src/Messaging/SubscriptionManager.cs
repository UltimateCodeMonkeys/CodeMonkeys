using CodeMonkeys.Core.Messaging;
using CodeMonkeys.Messaging.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CodeMonkeys.Messaging
{
    internal sealed class SubscriptionManager
    {
        private readonly HashSet<Subscription> _items;
        private readonly object _sync;

        private readonly CancellationToken _token;
        private readonly SubscriptionManagerOptions _options;

        internal SubscriptionManager(
            CancellationToken token,
            SubscriptionManagerOptions options = null)
        {
            _items = new HashSet<Subscription>();
            _sync = new object();

            _token = token;
            _options = options ?? new SubscriptionManagerOptions();

            Task.Run(RemoveCollectedSubscribers);
        }

        /// <inheritdoc/>
        internal IEnumerable<ISubscriberOf<TEvent>> GetSubscribersOf<TEvent>()
            where TEvent : class, IEvent
        {
            var subscriptions = _items
                .Where(subscription => subscription.EventType.Equals(typeof(TEvent)));

            return subscriptions
                .Select(subscription => subscription.Reference.Target as ISubscriberOf<TEvent>);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
