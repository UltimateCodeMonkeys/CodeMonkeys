using System.Runtime.CompilerServices;

namespace CodeMonkeys.Configuration
{
    public class Options
    {
        private readonly PropertyBag _propertyBag
            = new PropertyBag();


        protected Options()
        {
        }


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

        public TProperty GetValue<TProperty>(
            TProperty defaultValue,
            [CallerMemberName]string propertyName = "")
        {
            return _propertyBag.GetValue(
                defaultValue,
                propertyName);
        }
    }
}
