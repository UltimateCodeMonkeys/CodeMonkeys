using System.ComponentModel;

namespace CodeMonkeys.MVVM.PropertyChanged.Events
{
    public class AdvancedPropertyChangingEventArgs :
        PropertyChangingEventArgs
    {
        public object OldValue { get; set; }

        public object NewValue { get; set; }


        public bool Cancel { get; set; } = false;


        public AdvancedPropertyChangingEventArgs(
            string propertyName)
            : base(propertyName)
        {
        }
    }
}