using System.Threading.Tasks;

namespace CodeMonkeys.Core.Messaging
{
    public interface IEventAggregator
    {
        /// <summary>
        /// Publish's a specific event to all associated subscriber instances.
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <param name="event">The event instance.</param>
        void Publish<TEvent>(TEvent @event)
            where TEvent : class, IEvent;

        /// <summary>
        /// Publish's a specific event to all associated subscriber instances.
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <param name="event">The event instance.</param>
        Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : class, IEvent;
    }
}
