using Microsoft.Extensions.Primitives;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;

namespace CodeMonkeys.Configuration
{
    public abstract class Options
    {
        private readonly ConcurrentDictionary<string, object> _properties =
            new ConcurrentDictionary<string, object>();

        private OptionsChangeToken _token = new OptionsChangeToken();

        public IChangeToken GetChangeToken() => _token;

        protected void SetValue<TProperty>(
            TProperty value, 
            bool reload = true,
            [CallerMemberName]string propertyName = "")
        {
            // field = value;

            if (reload)
                Reload();
        }

        protected TProperty GetValue<TProperty>(
            [CallerMemberName]string propertyName = "")
        {
            return (TProperty)_properties.GetOrAdd(
                propertyName,
                default(TProperty));
        }

        private bool SetPropertyValue(
            string propertyName,
            object value)
        {
            var stored = _properties.AddOrUpdate(
                propertyName,
                value,
                (name, oldValue) => value);

            return stored == value;
        }

        private void Reload()
        {
            var previousToken = Interlocked.Exchange(ref _token, new OptionsChangeToken());
            previousToken.OnReload();
        }
    }
}
