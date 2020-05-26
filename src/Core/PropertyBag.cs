using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace CodeMonkeys
{
    public class PropertyBag
    {
        private readonly ConcurrentDictionary<string, object> _properties =
            new ConcurrentDictionary<string, object>();

        public bool SetValue<TProperty>(
            TProperty value,
            [CallerMemberName]string propertyName = "")
        {
            var stored = _properties.AddOrUpdate(
                propertyName,
                value,
                (name, oldValue) => value);

            return stored == value as object;
        }

        public TProperty GetValue<TProperty>(
            [CallerMemberName]string propertyName = "")
        {
            return (TProperty)_properties.GetOrAdd(
                propertyName,
                default(TProperty));
        }

        public TProperty GetValue<TProperty>(
            TProperty defaultValue,
            [CallerMemberName]string propertyName = "")
        {
            return (TProperty)_properties.GetOrAdd(
                propertyName,
                defaultValue);
        }
    }
}
