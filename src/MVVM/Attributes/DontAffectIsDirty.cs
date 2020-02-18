using System;

namespace CodeMonkeys.MVVM.Attributes
{
    /// <summary>
    /// Prevents a property from changing the IsDirty flag (only affects classes inherited from ModelBase)
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Property,
        AllowMultiple = false,
        Inherited = true)]
    public class DontAffectIsDirty :
        Attribute
    {

    }
}