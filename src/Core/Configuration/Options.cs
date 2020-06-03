using Microsoft.Extensions.Primitives;
using System.Runtime.CompilerServices;
using System.Threading;

namespace CodeMonkeys.Configuration
{
    public abstract class Options
    {
        private OptionsChangeToken _token = new OptionsChangeToken();

        private readonly PropertyBag _propertyBag
            = new PropertyBag();

        public IChangeToken GetChangeToken() => _token;

        protected void SetValueAndReload<TProperty>(
            TProperty value,
            [CallerMemberName]string propertyName = "")
        {
            SetValue(value, propertyName);
            Reload();
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

        private void Reload()
        {
            var previousToken = Interlocked.Exchange(ref _token, new OptionsChangeToken());
            previousToken.OnReload();
        }
    }
}
