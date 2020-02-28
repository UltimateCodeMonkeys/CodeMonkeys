using CodeMonkeys.Core.Helpers;
using CodeMonkeys.Core.Messaging;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMonkeys.Messaging
{
    public sealed class EventAggregator : IEventAggregator
    {
        private readonly List<Subscription> _subscriptions;
        private readonly object _sync;

        public EventAggregator()
        {
            _subscriptions = new List<Subscription>();
            _sync = new object();
        }

        public void Publish<TEvent>(TEvent @event)
            where TEvent : class, IEvent
        {
        }

        public Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : class, IEvent
        {
            Publish(@event);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Register<TEvent>(ISubscriberOf<TEvent> subscriber)
            where TEvent : class, IEvent
        {

        }
    }
}
