using CodeMonkeys.Core;
using CodeMonkeys.Core.Messaging;
using CodeMonkeys.Messaging.Caching;
using CodeMonkeys.Messaging.Configuration;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("UnitTests")]
namespace CodeMonkeys.Messaging
{
    public sealed class EventAggregator : IEventAggregator, IDisposable
    {
        private readonly IEventTypeCache _cache;

        private readonly ISubscriptionManager _subscriptionManager;

        public EventAggregator()
        {
            _cache = new EventTypeCache();
        }

        public EventAggregator(SubscriptionManagerOptions options = null)
            
            : this()
        {
            _subscriptionManager = new SubscriptionManager(options);
        }

        public EventAggregator(
            IEnumerable<ISubscriber> subscribers,
            SubscriptionManagerOptions options = null)
            
            : this(options)
        {
            Argument.NotNull(
                subscribers,
                nameof(subscribers));

            Register(subscribers);                
        }

        #region Unit Testing

        internal EventAggregator(ISubscriptionManager subscriptionManager)
            : this()
        {

            _subscriptionManager = subscriptionManager;
        }

        internal EventAggregator(
            ISubscriptionManager subscriptionManager,
            IEnumerable<ISubscriber> subscribers)
            
            : this()
        {
            Register(subscribers);
        } 

        #endregion

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

            var subscribers = _subscriptionManager.GetSubscribersOf<TEvent>();

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

            _subscriptionManager.Add(typeof(TEvent), subscriber);
        }

        /// <inheritdoc/>
        public void Register(ISubscriber subscriber)
        {
            Argument.NotNull(
                subscriber,
                nameof(subscriber));

            foreach (var type in _cache.GetOrAddEventTypesOf(subscriber))
                _subscriptionManager.Add(type, subscriber);
        }        

        /// <inheritdoc/>
        public void DeregisterFrom<TEvent>(ISubscriberOf<TEvent> subscriber)
            where TEvent : class, IEvent
        {
            Argument.NotNull(
                subscriber,
                nameof(subscriber));

            _subscriptionManager.Remove(typeof(TEvent), subscriber);
        }

        /// <inheritdoc/>
        public void Deregister(ISubscriber subscriber)
        {
            Argument.NotNull(
                subscriber,
                nameof(subscriber));

            foreach (var type in _cache.GetOrAddEventTypesOf(subscriber))
                _subscriptionManager.Remove(type, subscriber);
        }

        private void Register(IEnumerable<ISubscriber> subscribers)
        {
            foreach (var subscriber in subscribers)
            {
                try
                {
                    Register(subscriber);
                }
                catch { } // ignored
            }
        }

        public void Dispose()
        {
            _subscriptionManager.Dispose();
        }
    }
}
