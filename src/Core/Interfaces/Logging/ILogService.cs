using System;
using System.Runtime.CompilerServices;

namespace CodeMonkeys.Core.Interfaces.Logging
{
    public interface ILogService
    {
        /// <summary>
        /// Contains the unique identifier for this service. Needed for internal usage.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Contains the current log level of the service instance.
        /// </summary>
        LogLevel LogLevel { get; }

        /// <summary>
        /// The type to which this service is assigned.
        /// </summary>
        Type AssignedType { get; }

        /// <summary>
        /// Writes a log message with the level trace.
        /// </summary>
        /// <param name="message">The message which should be written.</param>
        void Trace(
            string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        /// Writes a log message with the level debug.
        /// </summary>
        /// <param name="message">The message which should be written.</param>
        void Debug(
            string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        /// Writes a log message with the level info.
        /// </summary>
        /// <param name="message">The message which should be written.</param>
        void Info(
            string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        /// Writes a log message with the level warning.
        /// </summary>
        /// <param name="message">The message which should be written.</param>
        void Warning(
            string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        /// Writes a log message with the level warning.
        /// </summary>
        /// <param name="message">The message which should be written.</param>
        /// <param name="exception">An occurred exception which should be written.</param>
        void Warning(
            string message,
            Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        /// Writes a log message with the level warning.
        /// </summary>
        /// <param name="exception">An occurred exception which should be written.</param>
        void Warning(
            Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        /// Writes a log message with the level error.
        /// </summary>
        /// <param name="message">The message which should be written.</param>
        void Error(
            string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        /// Writes a log message with the level error.
        /// </summary>
        /// <param name="exception">An occurred exception which should be written.</param>
        void Error(
            Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        /// Writes a log message with the level error.
        /// </summary>
        /// <param name="message">The message which should be written.</param>
        /// <param name="exception">An occurred exception which should be written.</param>
        void Error(
            string message,
            Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        /// Writes a log message with the level critical.
        /// </summary>
        /// <param name="message">The message which should be written.</param>
        void Critical(
            string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        /// Writes a log message with the level critical.
        /// </summary>
        /// <param name="exception">An occurred exception which should be written.</param>
        void Critical(
            Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = 0);

        /// <summary>
        /// Writes a log message with the level critical.
        /// </summary>
        /// <param name="message">The message which should be written.</param>
        /// <param name="exception">An occurred exception which should be written.</param>
        void Critical(
            string message,
            Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = 0);
    }
}