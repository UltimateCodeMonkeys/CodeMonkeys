﻿namespace CodeMonkeys.Logging.Console
{
    /// <summary>
    /// <see cref="IScopedLogService"/> which writes to <see cref="System.Console"/>.
    /// </summary>
    public sealed class ConsoleLogService : ScopedLogService<ConsoleLogOptions>
    {
        internal ConsoleLogService(string context)
            : base(context)
        {
            if (Options.ColorizeOutput)
            {
                MessageFormatter = new LogMessageColorizer();
            }
        }

        protected override void PublishMessage(LogMessage message)
        {
            System.Console.WriteLine(
                MessageFormatter.Format(
                        message,
                        Options.TimeStampFormat));
        }
    }
}
