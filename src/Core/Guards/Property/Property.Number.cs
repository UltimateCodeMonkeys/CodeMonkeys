using System.Runtime.CompilerServices;

namespace CodeMonkeys
{
    public static partial class Property
    {
        public static void GreaterThan(
            long? param,
            long? value,
            [CallerMemberName] string propertyName = "",
            string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
                message = $"Setter value for property '{propertyName}' is less than minimum.";

            Argument.GreaterThan(
                param, 
                value, 
                propertyName, 
                message);
        }
    }
}
