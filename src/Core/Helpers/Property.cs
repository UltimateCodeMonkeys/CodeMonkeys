using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CodeMonkeys.Core
{
    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    public static partial class Property
    {
        public static void NotDefault<T>(
            T param,
            [CallerMemberName] string propertyName = "",
            string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
                message = $"Setter value for property '{propertyName}' is default.";

            Argument.NotDefault(
                param, 
                propertyName, 
                message);
        }
    }
}
