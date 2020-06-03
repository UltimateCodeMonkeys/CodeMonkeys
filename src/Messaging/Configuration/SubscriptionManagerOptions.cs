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
            get => GetValue(TimeSpan.FromSeconds(10));
            set => SetValue(value);
        }
    }
}
