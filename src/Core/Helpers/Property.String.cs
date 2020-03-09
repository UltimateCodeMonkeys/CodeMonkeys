using System.Runtime.CompilerServices;

namespace CodeMonkeys.Core
{
    public static partial class Property
    {
        public static void NotEmptyOrWhiteSpace(
            string param,
            [CallerMemberName] string propertyName = "",
            string message = "")
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                message = $"Setter value for property '{propertyName}' is empty or whitespace.";

            Argument.NotEmptyOrWhiteSpace(
                param,
                propertyName,
                message);
        }
    }
}
