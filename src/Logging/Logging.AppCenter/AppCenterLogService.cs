using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

using System.Collections.Generic;

namespace CodeMonkeys.Logging.AppCenter
{
    public sealed class AppCenterLogService : ScopedLogService<AppCenterLogOptions>
    {
        internal AppCenterLogService(string context)
            : base(context)
        {
        }

        protected override void PublishMessage(LogMessage message)
        {
            var dictionary = ConvertMessageToDictionary(message);

            if (message.Exception == null)
            {
                Analytics.TrackEvent(message.Context, dictionary);
            }
            else
            {
                Crashes.TrackError(message.Exception, dictionary);
            }
        }

        private Dictionary<string, string> ConvertMessageToDictionary(LogMessage message)
        {
            // timestamp is not necessary because app center keeps track of it
            var properties = new Dictionary<string, string>
            {
                { nameof(message.LogLevel), message.LogLevel.ToString() },
                { nameof(message.Context), message.Context },
                { nameof(message.MethodName), message.MethodName },
                { nameof(message.Exception), message.Exception?.ToString() },
                { nameof(message.FormattedMessage), message.FormattedMessage }
            };

            return properties;
        }
    }
}
