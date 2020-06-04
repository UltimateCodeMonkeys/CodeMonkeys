using CodeMonkeys.Configuration;

using System;

namespace CodeMonkeys.Messaging
{
    public class SubscriptionManagerOptions : Options
    {
        private const int DEFAULT_FLUSHSUBSCRIPTIONSPERIOD = 10;

        /// <summary>
        /// Period to check for dead subscriptions
        /// </summary>
        public TimeSpan FlushSubscriptionsPeriod
        {
            get => GetValue(TimeSpan.FromSeconds(DEFAULT_FLUSHSUBSCRIPTIONSPERIOD));
            set => SetValue(value);
        }
    }
}
