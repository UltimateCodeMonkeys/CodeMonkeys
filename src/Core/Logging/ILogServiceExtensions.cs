using System;

namespace CodeMonkeys.Core.Logging
{
    public static class LogServiceExtensions
    {
        public static void Trace(
            this ILogService _this,
            string message) => _this.Log(LogLevel.Trace, message, null, null);

        public static void Trace(
            this ILogService _this,
            string message,
            Exception ex) => _this.Log(LogLevel.Trace, message, ex, null);

        public static void Debug(
            this ILogService _this,
            string message) => _this.Log(LogLevel.Debug, message, null, null);

        public static void Debug(
            this ILogService _this,
            string message,
            Exception ex) => _this.Log(LogLevel.Debug, message, ex, null);

        public static void Info(
            this ILogService _this,
            string message) => _this.Log(LogLevel.Info, message, null, null);

        public static void Info(
            this ILogService _this,
            string message,
            Exception ex) => _this.Log(LogLevel.Info, message, ex, null);

        public static void Warning(
            this ILogService _this,
            string message) => _this.Log(LogLevel.Warning, message, null, null);

        public static void Warning(
            this ILogService _this,
            Exception ex) => _this.Log<object>(LogLevel.Warning, null, ex, null);

        public static void Warning(
            this ILogService _this,
            string message,
            Exception ex) => _this.Log(LogLevel.Warning, message, ex, null);

        public static void Error(
            this ILogService _this,
            string message) => _this.Log(LogLevel.Error, message, null, null);

        public static void Error(
            this ILogService _this,
            Exception ex) => _this.Log<object>(LogLevel.Error, null, ex, null);

        public static void Error(
            this ILogService _this,
            string message,
            Exception ex) => _this.Log(LogLevel.Error, message, ex, null);

        public static void Critical(
            this ILogService _this,
            string message) => _this.Log(LogLevel.Critical, message, null, null);

        public static void Critical(
            this ILogService _this,
            Exception ex) => _this.Log<object>(LogLevel.Critical, null, ex, null);

        public static void Critical(
            this ILogService _this,
            string message,
            Exception ex) => _this.Log(LogLevel.Critical, message, ex, null);
    }
}
