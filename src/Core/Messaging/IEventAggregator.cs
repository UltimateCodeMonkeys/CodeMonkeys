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

        ///// <summary>
        ///// 
        ///// </summary>
        Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : class, IEvent;

        /// <summary>
        /// 
        /// </summary>
        void RegisterTo<TEvent>(ISubscriberOf<TEvent> subscriber)
            where TEvent : class, IEvent;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriber"></param>
        void Register(ISubscriber subscriber);

        /// <summary>
        /// 
        /// </summary>
        void DeregisterFrom<TEvent>(ISubscriberOf<TEvent> subscriber)
            where TEvent : class, IEvent;

        /// <summary>
        /// 
        /// </summary>
        void Deregister(ISubscriber subscriber);
    }
}
