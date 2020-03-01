using CodeMonkeys.Core;
using CodeMonkeys.Core.Messaging;
using CodeMonkeys.Messaging.Configuration;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CodeMonkeys.Messaging
{
    public sealed class EventAggregator : IEventAggregator, IDisposable
    {
        private readonly CancellationTokenSource _cts;
        private readonly SubscriptionRegistry _registry;
        private readonly ConcurrentDictionary<Type, IList<Type>> _eventTypeCache;

        public EventAggregator(SubscriptionOptions options = null)
        {
            _cts = new CancellationTokenSource();

            _registry = new SubscriptionRegistry(
                options ?? new SubscriptionOptions(),
                _cts.Token);

            _eventTypeCache = new ConcurrentDictionary<Type, IList<Type>>();
        }

        public EventAggregator(
            IEnumerable<ISubscriber> subscribers,
            SubscriptionOptions options = null) 
            
            : this(options)
        {
            Argument.NotNull(
                subscribers,
                nameof(subscribers));

            Register(subscribers);                
        }

        /// <inheritdoc/>
        public void Publish<TEvent>(TEvent @event)
            where TEvent : class, IEvent
        {
            Argument.NotNull(
                @event,
                nameof(@event));

            PublishAsync(@event);
        }

        /// <inheritdoc/>
        public async Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : class, IEvent
        {
            Argument.NotNull(
                @event,
                nameof(@event));

            var subscribers = _registry.GetSubscribersOf<TEvent>();

            foreach (var subscriber in subscribers)
                await subscriber.ReceiveEventAsync(@event);
        }

        /// <inheritdoc/>
        public void RegisterTo<TEvent>(ISubscriberOf<TEvent> subscriber)
            where TEvent : class, IEvent
        {
            Argument.NotNull(
                subscriber,
                nameof(subscriber));

            _registry.Add(typeof(TEvent), subscriber);
        }

        /// <inheritdoc/>
        public void Register(ISubscriber subscriber)
        {
            Argument.NotNull(
                subscriber,
                nameof(subscriber));

            foreach (var type in GetGenericTypeArgumentsOfSubscriber(subscriber))
                _registry.Add(type, subscriber);
        }

        private void Register(IEnumerable<ISubscriber> subscribers)
        {
            foreach (var subscriber in subscribers)
            {
                try
                {
                    Register(subscriber);
                }
                catch { }
            }
        }

        /// <inheritdoc/>
        public void DeregisterFrom<TEvent>(ISubscriberOf<TEvent> subscriber)
            where TEvent : class, IEvent
        {
            Argument.NotNull(
                subscriber,
                nameof(subscriber));

            _registry.Remove(typeof(TEvent), subscriber);
        }

        /// <inheritdoc/>
        public void Deregister(ISubscriber subscriber)
        {
            Argument.NotNull(
                subscriber,
                nameof(subscriber));

            foreach (var type in GetGenericTypeArgumentsOfSubscriber(subscriber))
                _registry.Remove(type, subscriber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IList<Type> GetGenericTypeArgumentsOfSubscriber(ISubscriber subscriber)
        {
            var type = subscriber.GetType();

            if (_eventTypeCache.TryGetValue(type, out var eventTypes))
                return eventTypes;

            var interfaces = type
                .GetInterfaces()
                .Where(@interface =>
                    @interface.IsGenericType &&
                    @interface.GetGenericTypeDefinition() == typeof(ISubscriberOf<>));

            eventTypes = interfaces
                .Select(@interface =>
                    @interface.GenericTypeArguments.FirstOrDefault())
                .ToList();

            _eventTypeCache.TryAdd(type, eventTypes);

            return eventTypes;
        }

        public void Dispose()
        {
            _cts.Cancel();
        }

        ~EventAggregator()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}
