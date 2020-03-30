using CodeMonkeys.Core.Messaging;

using System.Threading.Tasks;

namespace CodeMonkeys.UnitTests.Messaging
{
    internal class Subscriber : ISubscriberOf<Event>
    {
        public Task ReceiveEventAsync(Event @event)
        {
            return Task.CompletedTask;
        }
    }
}
