using System;

namespace CodeMonkeys.Messaging
{
    internal readonly struct Subscription
    {
        internal Type EventType { get; }

        internal WeakReference Reference { get; }

        private Subscription(Type eventType, WeakReference @ref)
        {
            Argument.NotNull(
                eventType,
                nameof(eventType));

            Argument.NotNull(
                @ref,
                nameof(@ref));

            EventType = eventType;
            Reference = @ref;
        }

        internal static Subscription Create(Type eventType, ISubscriber subscriber) => 
            new Subscription(eventType, new WeakReference(subscriber));

        internal static Subscription Create<TEvent>(ISubscriber subscriber)
            where TEvent : class, IEvent => Create(typeof(TEvent), subscriber);

        public override bool Equals(object obj)
        {
            var other = (Subscription)obj;

            return Reference.Target == other.Reference.Target &&
                   EventType == other.EventType;
        }

        public override int GetHashCode()
        {
            return Reference.Target.GetHashCode();
        }
    }
}
