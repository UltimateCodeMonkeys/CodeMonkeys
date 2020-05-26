using Microsoft.Extensions.Primitives;
using System.Runtime.CompilerServices;
using System.Threading;

namespace CodeMonkeys.Configuration
{
    public abstract class Options : PropertyBag
    {
        private OptionsChangeToken _token = new OptionsChangeToken();

        public IChangeToken GetChangeToken() => _token;

        protected void SetValueAndReload<TProperty>(
            TProperty value,
            [CallerMemberName]string propertyName = "")
        {
            SetValue(value, 
                propertyName: propertyName);

            Reload();
        }

        private void Reload()
        {
            var previousToken = Interlocked.Exchange(ref _token, new OptionsChangeToken());
            previousToken.OnReload();
        }
    }
}
