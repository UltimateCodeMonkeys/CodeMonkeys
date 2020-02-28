using System;

namespace CodeMonkeys.Core
{
    public static partial class Argument
    {
        /// <summary>
        /// Throws a <see cref="ArgumentNullException"/> when the parameter value equals <see langword="null"/>.
        /// Throws a <see cref="ArgumentException"/> when the <see cref="string"/> parameter equals empty or whitespace.
        /// </summary>
        public static void NotEmptyOrWhitespace(
            string param,
            string paramName,
            string message = "")
        {
            NotNull(param, nameof(param));

            if (!string.IsNullOrEmpty(param))
                return;

            if (string.IsNullOrWhiteSpace(message))
                message = $"'{paramName}' is empty or whitespace.";

            throw new ArgumentException(
                message,
                paramName);
        }
    }
}
