using CodeMonkeys.Core.Messaging;
using System.Collections.Generic;

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
    }
}
