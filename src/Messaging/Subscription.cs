using CodeMonkeys.Core;
using CodeMonkeys.Core.Messaging;

using System;

namespace CodeMonkeys.Messaging
{
    internal readonly struct Subscription
    {
        internal Type EventType { get; }

        internal WeakReference Ref { get; }

        private Subscription(Type eventType, WeakReference @ref)
        {
            Argument.NotNull(
                eventType,
                nameof(eventType));

            Argument.NotNull(
                @ref,
                nameof(@ref));

            EventType = eventType;
            Ref = @ref;
        }

        internal static Subscription Create(Type eventType, ISubscriber subscriber) => 
            new Subscription(eventType, new WeakReference(subscriber));

        internal static Subscription Create<TEvent>(ISubscriber subscriber)
            where TEvent : class, IEvent => Create(typeof(TEvent), subscriber);
    }
}
