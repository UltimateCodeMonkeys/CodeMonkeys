using System;
using System.Runtime.CompilerServices;

namespace CodeMonkeys.Logging
{
    public static partial class LogServiceExtensions
    {
        public static void Error<TState>(
            this ILogService service,
            DateTimeOffset timestamp,
            TState state,
            Exception ex,
            [CallerMemberName] string methodName = "")
        {
            service?.Internal(
                timestamp,
                state,
                ex,
                methodName);
        }

        public static void Error<TState>(
            this ILogService service,
            DateTimeOffset timestamp,
            TState state,
            [CallerMemberName] string methodName = "")
        {
            service?.Internal(
                timestamp,
                state,
                methodName);
        }

        public static void Error(
            this ILogService service,
            DateTimeOffset timestamp,
            Exception ex,
            [CallerMemberName] string methodName = "")
        {
            service?.Internal(
                timestamp,
                ex,
                methodName);
        }



        public static void Error<TState>(
            this ILogService service,
            TState message,
            Exception ex,
            [CallerMemberName] string methodName = "")
        {
            service?.Internal(
                message,
                ex,
                methodName);
        }

        public static void Error<TState>(
            this ILogService service,
            TState message,
            [CallerMemberName] string methodName = "")
        {
            service?.Internal(
                message,
                methodName);
        }

        public static void Error(
            this ILogService service,
            Exception ex,
            [CallerMemberName] string methodName = "")
        {
            service?.Internal(
                ex,
                methodName);
        }



        public static void Error<TState>(
            this ILogService service,
            TState message,
            Exception ex,
            Func<TState, Exception, string> formatter,
            [CallerMemberName] string methodName = "")
        {
            service?.Internal(
                message,
                ex,
                formatter,
                methodName);
        }

        public static void Error<TState>(
            this ILogService service,
            TState message,
            Func<TState, Exception, string> formatter,
            [CallerMemberName] string methodName = "")
        {
            service?.Internal(
                message,
                formatter,
                methodName);
        }

        public static void Error(
            this ILogService service,
            Exception ex,
            Func<object, Exception, string> formatter,
            [CallerMemberName] string methodName = "")
        {
            service?.Internal(
                ex,
                formatter,
                methodName);
        }



        public static void Error<TState>(
            this ILogService service,
            DateTimeOffset timestamp,
            TState message,
            Func<TState, Exception, string> formatter,
            [CallerMemberName] string methodName = "")
        {
            service?.Internal(
                timestamp,
                message,
                formatter,
                methodName);
        }

        public static void Error(
            this ILogService service,
            DateTimeOffset timestamp,
            Exception ex,
            Func<object, Exception, string> formatter,
            [CallerMemberName] string methodName = "")
        {
            service?.Internal(
                timestamp,
                ex,
                formatter,
                methodName);
        }

        public static void Error<TState>(
            this ILogService service,
            DateTimeOffset timestamp,
            TState message,
            Exception ex,
            Func<TState, Exception, string> formatter,
            [CallerMemberName] string methodName = "")
        {
            service?.Internal(
                timestamp,
                message,
                ex,
                formatter,
                methodName);
        }
    }
}
