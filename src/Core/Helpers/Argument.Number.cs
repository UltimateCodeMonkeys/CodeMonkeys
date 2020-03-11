using System;

namespace CodeMonkeys.Core
{
    public static partial class Argument
    {
        public static void Min(
            int? param,
            int? value,
            string paramName,
            string message = "")
        {
            if (param <= value)
                return;

            if (string.IsNullOrWhiteSpace(message))
                message = $"Parameter '{paramName}' is less than minimum value.";

            throw new ArgumentException(
                message,
                paramName);
        }
    }
}
