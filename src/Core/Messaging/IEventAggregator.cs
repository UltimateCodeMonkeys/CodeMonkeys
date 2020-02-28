using System.Threading.Tasks;

namespace CodeMonkeys.Core.Messaging
{
    public interface IEventAggregator
    {
        /// <summary>
        /// 
        /// </summary>
        void Publish<TEvent>(TEvent @event)
            where TEvent : class, IEvent;

        /// <summary>
        /// 
        /// </summary>
        Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : class, IEvent;

        /// <summary>
        /// 
        /// </summary>
        void AddRegistration<TEvent>(ISubscriberOf<TEvent> subscriber)
            where TEvent : class, IEvent;

        /// <summary>
        /// 
        /// </summary>
        void RemoveRegistration<TEvent>(ISubscriberOf<TEvent> subscriber)
            where TEvent : class, IEvent;
    }
}
