using System;

namespace CodeMonkeys.Messaging
{
    internal readonly struct Subscription
    {
        internal Type EventType { get; }

        internal WeakReference Ref { get; }

        internal Subscription(Type eventType, WeakReference @ref)
        {
            EventType = eventType;
            Ref = @ref;
        }
    }
}
