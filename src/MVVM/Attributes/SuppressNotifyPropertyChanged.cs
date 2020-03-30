using System;

namespace CodeMonkeys.MVVM.Attributes
{
    /// <inheritdoc />
    /// <summary>
    /// If applied to a property, prevents the PropertyChanged event from being raised
    /// <para />If applied to a class, prevents the PropertyChanged event from being raised for ALL properties
    /// <para />Can be used in common to e.g. exclude all but one
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Class,
        AllowMultiple = false,
        Inherited = true)]
    public class SuppressNotifyPropertyChanged :
        Attribute
    {
        /// <summary>
        /// Shall PropertyChanged event raising be suppressed?
        /// </summary>
        public bool Suppress { get; }

        public SuppressNotifyPropertyChanged()
            : this(true)
        { }

        public SuppressNotifyPropertyChanged(
            bool shallSuppress)
        {
            Suppress = shallSuppress;
        }
    }
}