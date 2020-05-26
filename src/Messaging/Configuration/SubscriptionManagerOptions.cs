using CodeMonkeys.Configuration;

using System;

namespace CodeMonkeys.Messaging
{
    public class SubscriptionManagerOptions : Options
    {
        /// <summary>
        /// Period to check for dead subscriptions
        /// </summary>
        public TimeSpan FlushSubscriptionsPeriod
        {
            get => GetValue<TimeSpan>();
            set => SetValue(value);
        }

        public SubscriptionManagerOptions()
        {
            FlushSubscriptionsPeriod = TimeSpan.FromSeconds(10);
        }
    }
}
