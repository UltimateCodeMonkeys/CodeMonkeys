using System;
using System.Runtime.CompilerServices;

namespace CodeMonkeys.Logging
{
    public static partial class LogServiceExtensions
    {
        /// <summary>
        /// Wrapper for <see cref="ILogServiceBase.Log{TState}(DateTimeOffset, LogLevel, TState, string)"./>
        /// </summary>
        private static void Internal<TState>(
            this ILogService service,
            DateTimeOffset timestamp,
            TState state,
            Exception ex,
            string methodName = "",
            [CallerMemberName] string extensionMethodName = "")
        {
            service?.Internal(
                timestamp,
                state,
                ex,
                null,
                methodName,
                extensionMethodName);
        }

        /// <summary>
        /// Wrapper for <see cref="ILogServiceBase.Log{TState}(DateTimeOffset, LogLevel, TState, string)"./>
        /// </summary>
        private static void Internal<TState>(
            this ILogService service,
            DateTimeOffset timestamp,
            TState state,
            string methodName = "",
            [CallerMemberName] string extensionMethodName = "")
        {
            service?.Internal(
                timestamp,
                state,
                null,
                null,
                methodName,
                extensionMethodName);
        }

        /// <summary>
        /// Wrapper for <see cref="ILogServiceBase.Log{TState}(DateTimeOffset, LogLevel, TState, string)"./>
        /// </summary>
        private static void Internal(
            this ILogService service,
            DateTimeOffset timestamp,
            Exception ex,
            string methodName = "",
            [CallerMemberName] string extensionMethodName = "")
        {
            service?.Internal<object>(
                timestamp,
                null,
                ex,
                null,
                methodName,
                extensionMethodName);
        }



        /// <summary>
        /// Wrapper for <see cref="ILogServiceBase.Log{TState}(LogLevel, TState, string)"./>
        /// </summary>
        private static void Internal<TState>(
            this ILogService service,
            TState message,
            Exception ex,
            string methodName = "",
            [CallerMemberName] string extensionMethodName = "")
        {
            service?.Internal(
                DateTimeOffset.Now,
                message,
                ex,
                null,
                methodName,
                extensionMethodName);
        }

        /// <summary>
        /// Wrapper for <see cref="ILogServiceBase.Log{TState}(LogLevel, TState, string)"./>
        /// </summary>
        private static void Internal<TState>(
            this ILogService service,
            TState message,
            string methodName = "",
            [CallerMemberName] string extensionMethodName = "")
        {
            service?.Internal(
                DateTimeOffset.Now,
                message,
                null,
                null,
                methodName,
                extensionMethodName);
        }

        /// <summary>
        /// Wrapper for <see cref="ILogServiceBase.Log{TState}(LogLevel, TState, string)"./>
        /// </summary>
        private static void Internal(
            this ILogService service,
            Exception ex,
            string methodName = "",
            [CallerMemberName] string extensionMethodName = "")
        {
            service?.Internal<object>(
                DateTimeOffset.Now,
                null,
                ex,
                null,
                methodName,
                extensionMethodName);
        }



        /// <summary>
        /// Wrapper for <see cref="ILogServiceBase.Log{TState}(LogLevel, TState, Exception, Func{TState, Exception, string}, string)"./>
        /// </summary>
        private static void Internal<TState>(
            this ILogService service,
            TState message,
            Exception ex,
            Func<TState, Exception, string> formatter,
            string methodName = "",
            [CallerMemberName] string extensionMethodName = "")
        {
            service?.Internal(
                DateTimeOffset.Now,
                message,
                ex,
                formatter,
                methodName,
                extensionMethodName);
        }

        /// <summary>
        /// Wrapper for <see cref="ILogServiceBase.Log{TState}(LogLevel, TState, Exception, Func{TState, Exception, string}, string)"./>
        /// </summary>
        private static void Internal<TState>(
            this ILogService service,
            TState message,
            Func<TState, Exception, string> formatter,
            string methodName = "",
            [CallerMemberName] string extensionMethodName = "")
        {
            service?.Internal(
                DateTimeOffset.Now,
                message,
                null,
                formatter,
                methodName,
                extensionMethodName);
        }

        /// <summary>
        /// Wrapper for <see cref="ILogServiceBase.Log{TState}(LogLevel, TState, Exception, Func{TState, Exception, string}, string)"./>
        /// </summary>
        private static void Internal(
            this ILogService service,
            Exception ex,
            Func<object, Exception, string> formatter,
            string methodName = "",
            [CallerMemberName] string extensionMethodName = "")
        {
            service?.Internal(
                DateTimeOffset.Now,
                null,
                ex,
                formatter,
                methodName,
                extensionMethodName);
        }



        /// <summary>
        /// Wrapper for <see cref="ILogServiceBase.Log{TState}(DateTimeOffset, LogLevel, TState, Exception, Func{TState, Exception, string}, string)"./>
        /// </summary>
        private static void Internal<TState>(
            this ILogService service,
            DateTimeOffset timestamp,
            TState message,
            Func<TState, Exception, string> formatter,
            string methodName = "",
            [CallerMemberName] string extensionMethodName = "")
        {
            service?.Internal(
                timestamp,
                message,
                null,
                formatter,
                methodName,
                extensionMethodName);
        }

        /// <summary>
        /// Wrapper for <see cref="ILogServiceBase.Log{TState}(DateTimeOffset, LogLevel, TState, Exception, Func{TState, Exception, string}, string)"./>
        /// </summary>
        private static void Internal(
            this ILogService service,
            DateTimeOffset timestamp,
            Exception ex,
            Func<object, Exception, string> formatter,
            string methodName = "",
            [CallerMemberName] string extensionMethodName = "")
        {
            service?.Internal(
                timestamp,
                null,
                ex,
                formatter,
                methodName,
                extensionMethodName);
        }

        /// <summary>
        /// Wrapper for <see cref="ILogServiceBase.Log{TState}(DateTimeOffset, LogLevel, TState, Exception, Func{TState, Exception, string}, string)"./>
        /// </summary>
        private static void Internal<TState>(
            this ILogService service,
            DateTimeOffset timestamp,
            TState message,
            Exception ex,
            Func<TState, Exception, string> formatter,
            string methodName = "",
            [CallerMemberName] string extensionMethodName = "")
        {
            if (!TryParseLogLevel(extensionMethodName, out var logLevel))
                return;

            service?.Log(
                timestamp,
                logLevel,
                message,
                ex,
                formatter,
                methodName);
        }

        private static bool TryParseLogLevel(string input, out LogLevel logLevel)
        {
            logLevel = LogLevel.Trace;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            if (!Enum.TryParse<LogLevel>(input, out var level))
                return false;

            logLevel = level;

            return true;
        }
    }
}
