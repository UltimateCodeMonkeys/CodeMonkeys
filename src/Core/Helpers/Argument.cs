using System;

namespace CodeMonkeys.Core.Helpers
{
    /// <summary>
    /// Contains various helper methods for verifying method parameters.
    /// </summary>
    public static class Argument
    {
        /// <summary>
        /// Throws a <see cref="ArgumentException"/> if the parameter is a empty guid.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="paramName">The parameter name.</param>
        /// <param name="message">The <see cref="ArgumentException"/> exception message.</param>
        public static void IsNotEmptyGuid(
            Guid param,
            string paramName = "",
            string message = "")
        {
            if (!param.Equals(Guid.Empty))
                return;

            throw new ArgumentException(
                paramName,
                message);
        }

        /// <summary>
        /// Throws a <see cref="ArgumentException"/> if the parameter is zero or negative.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="paramName">The parameter name.</param>
        /// <param name="message">The <see cref="ArgumentException"/> exception message.</param>
        public static void IsNotZeroOrNegative(
            int param,
            string paramName = "",
            string message = "")
        {
            if (param > 0)
                return;

            throw new ArgumentException(
                paramName,
                message);
        }

        /// <summary>
        /// Throws a <see cref="ArgumentNullException"/> if the parameter is null.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="paramName">The parameter name.</param>
        /// <param name="message">The <see cref="ArgumentNullException"/> exception message.</param>
        public static void IsNotNull(
            object param,
            string paramName = "",
            string message = "")
        {
            if (param == null)
                throw new ArgumentNullException(paramName, message);
        }

        /// <summary>
        /// Throws a <see cref="ArgumentNullException"/> if the parameter is null. If the parameter
        /// is empty it throws a <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="paramName">The parameter name.</param>
        /// <param name="message">The exception message.</param>
        public static void IsNotNullOrEmpty(
            string param,
            string paramName = "",
            string message = "")
        {
            IsNotNull(param, paramName, message);

            if (string.IsNullOrEmpty(param))
                throw new ArgumentException(paramName, message);
        }

        /// <summary>
        /// Throws a <see cref="ArgumentNullException"/> if the parameter is null. If the parameter
        /// is a whitespace it throws a <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="paramName">The parameter name.</param>
        /// <param name="message">The exception message.</param>
        public static void IsNotNullOrWhiteSpace(
            string param,
            string paramName = "",
            string message = "")
        {
            IsNotNull(param, paramName, message);

            if (string.IsNullOrWhiteSpace(param))
                throw new ArgumentException(message, paramName);
        }
    }
}