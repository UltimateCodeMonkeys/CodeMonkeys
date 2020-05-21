using System;
using System.Collections.Generic;

namespace CodeMonkeys
{
    /// <summary>
    /// Contains various helper methods for verifying method parameters.
    /// </summary>

#if (!DEBUG)
    [System.Diagnostics.DebuggerStepThrough]
    [System.Diagnostics.DebuggerNonUserCode]
#endif
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
        {
            if (!EqualityComparer<T>.Default.Equals(param, default))
                return;

            if (string.IsNullOrWhiteSpace(message))
                message = $"Parameter '{paramName}' is default.";

            throw new ArgumentException(
                message,
                paramName);
        }
    }
}