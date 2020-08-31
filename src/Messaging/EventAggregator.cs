using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeMonkeys.Messaging
{
    public sealed partial class EventAggregator : IEventAggregator
    {
        /// <inheritdoc/>
        public void Publish<TEvent>(TEvent @event)
            where TEvent : class, IEvent
        {
            Argument.NotNull(
                @event,
                nameof(@event));

            _ = PublishAsync(@event);
        }

        /// <inheritdoc/>
        public async Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : class, IEvent
        {
            Argument.NotNull(
                @event,
                nameof(@event));

            var subscribers = GetSubscribersOf<TEvent>();

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

            AddSubscriber(
                typeof(TEvent),
                subscriber);
        }

        /// <inheritdoc/>
        public void Register(ISubscriber subscriber)
        {
            Argument.NotNull(
                subscriber,
                nameof(subscriber));

            foreach (var eventType in GetOrAddEventTypesOf(subscriber))
            {
                AddSubscriber(
                    eventType,
                    subscriber);
            }
        }        

        /// <inheritdoc/>
        public void DeregisterFrom<TEvent>(ISubscriberOf<TEvent> subscriber)
            where TEvent : class, IEvent
        {
            Argument.NotNull(
                subscriber,
                nameof(subscriber));

            RemoveSubscriber(
                typeof(TEvent),
                subscriber);
        }

        /// <inheritdoc/>
        public void Deregister(ISubscriber subscriber)
        {
            Argument.NotNull(
                subscriber,
                nameof(subscriber));

            foreach (var eventType in GetOrAddEventTypesOf(subscriber))
            {
                RemoveSubscriber(
                    eventType,
                    subscriber);
            }
        }

        private IEnumerable<ISubscriberOf<TEvent>> GetSubscribersOf<TEvent>()
            where TEvent : class, IEvent
        {
            var subscriptions = _subscriptions
                .Where(subscription => subscription.EventType.Equals(typeof(TEvent)));

            return subscriptions
                .Select(subscription => subscription.Reference.Target as ISubscriberOf<TEvent>);
        }
    }
}
