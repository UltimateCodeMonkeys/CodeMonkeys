using System.Runtime.CompilerServices;

namespace CodeMonkeys.Core
{
    public static partial class Property
    {
        public static void Min(
            int param,
            int value,
            [CallerMemberName] string propertyName = "",
            string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
                message = $"Setter value for property '{propertyName}' is less than minimum.";

            Argument.Min(
                param, 
                value, 
                propertyName, 
                message);
        }
    }
}
