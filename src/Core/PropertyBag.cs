using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace CodeMonkeys
{
    public abstract class PropertyBag
    {
        protected readonly ConcurrentDictionary<string, object> Properties =
            new ConcurrentDictionary<string, object>();

        protected virtual void SetValue<TProperty>(
            TProperty value,
            [CallerMemberName]string propertyName = "")
        {
            Properties.AddOrUpdate(
                propertyName,
                value,
                (name, oldValue) => value);
        }

        protected virtual TProperty GetValue<TProperty>(
            [CallerMemberName]string propertyName = "")
        {
            return (TProperty)Properties.GetOrAdd(
                propertyName,
                default(TProperty));
        }
    }
}
