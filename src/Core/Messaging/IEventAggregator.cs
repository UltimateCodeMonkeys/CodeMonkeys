using System.Threading.Tasks;

namespace CodeMonkeys.Messaging
{
    public interface IEventAggregator
    {
        /// <summary>
        /// Publish's a specific event to all associated subscriber instances.
        /// </summary>
        void Publish<TEvent>(TEvent @event)
            where TEvent : class, IEvent;

        ///// <summary>
        ///// Publish's a specific event to all associated subscriber instances.
        ///// </summary>
        Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : class, IEvent;

        /// <summary>
        /// Registers a specific event type to the subscriber instance. Must implement <see cref="ISubscriberOf{TEvent}"/>.
        /// </summary>
        void RegisterTo<TEvent>(ISubscriberOf<TEvent> subscriber)
            where TEvent : class, IEvent;

        /// <summary>
        /// Registers all event types to the subscriber instance based on <see cref="ISubscriberOf{TEvent}"/> implementations.
        /// </summary>
        /// <param name="subscriber"></param>
        void Register(ISubscriber subscriber);

        /// <summary>
        /// Deregisters a specific event type from the subscriber instance. Must implement <see cref="ISubscriberOf{TEvent}"/>.
        /// </summary>
        void DeregisterFrom<TEvent>(ISubscriberOf<TEvent> subscriber)
            where TEvent : class, IEvent;

        /// <summary>
        /// Deregisters all event types from the subscriber instance based on <see cref="ISubscriberOf{TEvent}"/> implementations.
        /// </summary>
        void Deregister(ISubscriber subscriber);
    }
}
