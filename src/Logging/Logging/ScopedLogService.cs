﻿using CodeMonkeys.Configuration;

using System;

namespace CodeMonkeys.Logging
{
    public abstract class ScopedLogService<TOptions> : OptionsConsumer<TOptions>, IScopedLogService
        where TOptions : LogOptions, new()
    {
        /// <summary>
        /// Flag which indicates if the service accepts and queues writes.
        /// <para>Defaults to <see langword="true"/>.</para>
        /// </summary>
        public virtual bool IsEnabled { get; protected set; } = true;

        protected string Context { get; private set; }
        protected LogMessageFormatter MessageFormatter { get; set; }

        protected ScopedLogService(string context)
        {
            Context = context;
            MessageFormatter = new LogMessageFormatter();
        }

        public bool IsEnabledFor(LogLevel logLevel)
        {
            if (IsEnabled)
            {
                return logLevel >= Options.MinLevel;
            }

            return false;
        }

        public virtual void EnableLogging() => 
            IsEnabled = true;

        public virtual void DisableLogging() => 
            IsEnabled = false;


        public void Log<TState>(
            DateTimeOffset timestamp,
            LogLevel logLevel,
            TState state,
            string methodName = "")
        {
            Log(timestamp, logLevel, state, null, null, methodName);
        }

        public void Log<TState>(
            LogLevel logLevel,
            TState state,
            string methodName = "")
        {
            Log(DateTimeOffset.Now, logLevel, state, null, null, methodName);
        }

        public void Log<TState>(
            LogLevel logLevel,
            TState state,
            Exception ex,
            Func<TState, Exception, string> formatter,
            string methodName = "")
        {
            Log(DateTimeOffset.Now, logLevel, state, ex, formatter);
        }

        public void Log<TState>(
            DateTimeOffset timestamp,
            LogLevel logLevel,
            TState state,
            Exception ex,
            Func<TState, Exception, string> formatter,
            string methodName = "")
        {
            Argument.NotNull(formatter, nameof(formatter));

            var message = CreateMessage(timestamp,
                logLevel,
                state,
                ex,
                formatter,
                methodName);

            PublishMessage(message);
        }

        protected abstract void PublishMessage(LogMessage message);

        private LogMessage CreateMessage<TState>(DateTimeOffset timestamp,
            LogLevel logLevel,
            TState state,
            Exception ex,
            Func<TState, Exception, string> formatter,
            string methodName)
        {
            var message = new LogMessage(
                timestamp,
                logLevel,
                formatter(state, ex),
                Context,
                methodName,
                ex);

            return message;
        }
    }
}
