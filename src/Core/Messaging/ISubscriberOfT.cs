using System.Threading.Tasks;

namespace CodeMonkeys.Core.Messaging
{
    public interface ISubscriberOf<TEvent> : ISubscriber
        where TEvent : class, IEvent
    {
        Task ReceiveEventAsync(TEvent @event);
    }
}
