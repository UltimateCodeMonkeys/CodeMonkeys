using CodeMonkeys.Core;
using CodeMonkeys.Core.Messaging;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeMonkeys.Messaging
{
    public sealed class EventAggregator : IEventAggregator
    {
        private readonly List<Subscription> _subscriptions;
        private readonly object _sync;

        private readonly ConcurrentDictionary<Type, IList<Type>> _eventTypeCache;


        public EventAggregator()
        {
            _subscriptions = new List<Subscription>();
            _sync = new object();

            _eventTypeCache = new ConcurrentDictionary<Type, IList<Type>>();
        }

        public EventAggregator(IEnumerable<ISubscriber> subscribers) : this()
        {
            foreach (var subscriber in subscribers)
                AddRegistrations(subscriber);
        }

        /// <inheritdoc />
        public void Publish<TEvent>(TEvent @event)
            where TEvent : class, IEvent
        {
            Argument.NotNull(
                @event,
                nameof(@event));
        }

        /// <inheritdoc />
        public Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : class, IEvent
        {
            Publish(@event);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void AddRegistration<TEvent>(ISubscriberOf<TEvent> subscriber)
            where TEvent : class, IEvent
        {
            Argument.NotNull(
                subscriber,
                nameof(subscriber));

            AddRegistration(
                typeof(TEvent), 
                subscriber);
        }

        private void AddRegistration(
            Type eventType,
            ISubscriber subscriber)
        {
            var sub = Subscription.Create(eventType, subscriber);

            if (_subscriptions.Contains(sub))
                return;

            lock (_sync)
                _subscriptions.Add(sub);
        }

        /// <inheritdoc />
        public void AddRegistrations(ISubscriber subscriber)
        {
            Argument.NotNull(
                subscriber,
                nameof(subscriber));

            var eventTypes = GetEventTypesForSubscriber(subscriber);

            foreach (var type in eventTypes)
            {
                AddRegistration(type, subscriber);
            }
        }

        /// <inheritdoc />
        public void RemoveRegistration<TEvent>(ISubscriberOf<TEvent> subscriber)
            where TEvent : class, IEvent
        {
            Argument.NotNull(
                subscriber,
                nameof(subscriber));

            var sub = Subscription.Create<TEvent>(subscriber);

            lock (_sync)
                _subscriptions.Remove(sub);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IList<Type> GetEventTypesForSubscriber(ISubscriber subscriber)
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
    }
}
