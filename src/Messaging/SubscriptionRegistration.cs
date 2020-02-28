using CodeMonkeys.Core.Messaging;
using System.Collections.Generic;

namespace CodeMonkeys.Messaging
{
    internal class SubscriptionRegistration
    {
        private readonly List<Subscription> _list;
        private readonly object _sync;

        internal SubscriptionRegistration()
        {
            _list = new List<Subscription>();
            _sync = new object();
        }
    }
}
