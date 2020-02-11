namespace CodeMonkeys.MVVM.PropertyChanged.Events
{
    public class AdvancedPropertyChangingEventArgs<TProperty> :
        AdvancedPropertyChangingEventArgs
    {
        public new TProperty OldValue { get; set; }

        public new TProperty NewValue { get; set; }



        public AdvancedPropertyChangingEventArgs(
            string propertyName)
            : base(propertyName)
        {
        }

        public AdvancedPropertyChangingEventArgs(
            string propertyName,
            TProperty oldValue,
            TProperty newValue)
            : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}