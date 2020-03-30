using System.Threading.Tasks;

namespace CodeMonkeys.Core.Messaging
{
    /// <summary>
    /// Interface for a event specific subscriber.
    /// </summary>
    public interface ISubscriberOf<TEvent> : ISubscriber
        where TEvent : class, IEvent
    {
        /// <summary>
        /// Is invoked when the specific event occured.
        /// </summary>
        Task ReceiveEventAsync(TEvent @event);
    }
}
