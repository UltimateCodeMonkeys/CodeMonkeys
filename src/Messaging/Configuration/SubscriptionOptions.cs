using CodeMonkeys.Core.Configuration;

using System;

namespace CodeMonkeys.Messaging.Configuration
{
    public class SubscriptionOptions : Options
    {
        private TimeSpan _flushSubscriptionsPeriod = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Period to check for dead subscriptions
        /// </summary>
        public TimeSpan FlushSubscriptionsPeriod
        {
            get => _flushSubscriptionsPeriod;
            set => SetValue(ref _flushSubscriptionsPeriod, value);
        }
    }
}
