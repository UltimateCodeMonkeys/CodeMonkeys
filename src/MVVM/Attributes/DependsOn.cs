using System;

namespace CodeMonkeys.MVVM.Attributes
{
    /// <summary>
    /// States that a property's value depends on another property (e.g. calculated values like age)
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Property,
        AllowMultiple = true,
        Inherited = true)]
    public class DependsOn :
        Attribute
    {
        /// <summary>
        /// The source of this property its value
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName">Value source</param>
        public DependsOn(
            string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}