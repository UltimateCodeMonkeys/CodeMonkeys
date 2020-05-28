using System.Runtime.CompilerServices;

namespace CodeMonkeys.Configuration
{
    public abstract class Options
    {
        private readonly PropertyBag _propertyBag
            = new PropertyBag();

        protected bool SetValue<TProperty>(
            TProperty value,
            [CallerMemberName]string propertyName = "")
        {
            return _propertyBag.SetValue(
                value,
                propertyName);
        }

        public TProperty GetValue<TProperty>(
            [CallerMemberName]string propertyName = "")
        {
            return _propertyBag.GetValue<TProperty>(
                propertyName);
        }
    }
}
