using System.Runtime.CompilerServices;

namespace CodeMonkeys
{
#if (!DEBUG)
    [System.Diagnostics.DebuggerStepThrough]
    [System.Diagnostics.DebuggerNonUserCode]
#endif
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
