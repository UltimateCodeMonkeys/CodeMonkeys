using System;
using System.Diagnostics;

namespace CodeMonkeys.Core
{
    /// <summary>
    /// Contains various helper methods for verifying method parameters.
    /// </summary>
    [DebuggerStepThrough]
    [DebuggerNonUserCode]
    public static partial class Argument
    {
        /// <summary>
        /// Throws a <see cref="ArgumentNullException"/> when the parameter value equals <see langword="null"/>.
        /// </summary>
        public static void NotNull<T>(
            T param,
            string paramName,
            string message = "")

            where T : class
        {
            if (param != null)
                return;

            if (string.IsNullOrWhiteSpace(message))
                message = $"Parameter '{paramName}' is null.";

            throw new ArgumentNullException(
                paramName,
                message);
        }

        /// <summary>
        /// Throws a <see cref="ArgumentException"/> when the parameter value equals the default value.
        /// </summary>
        public static void NotDefault<T>(
            T param,
            string paramName,
            string message = "")

            where T : class
        {
            if (param != default(T))
                return;

            if (string.IsNullOrWhiteSpace(message))
                message = $"Parameter '{paramName}' is default value.";

            throw new ArgumentException(
                message,
                paramName);
        }
    }
}