using Microsoft.Extensions.Primitives;

using System.Threading;

namespace CodeMonkeys.Configuration
{
    public abstract class Options
    {
        private OptionsChangeToken _token = new OptionsChangeToken();

        public IChangeToken GetChangeToken() => _token;

        protected void SetValue<T>(
            ref T field, 
            T value, 
            bool reload = true)
        {
            field = value;

            if (reload)
                Reload();
        }

        private void Reload()
        {
            var previousToken = Interlocked.Exchange(ref _token, new OptionsChangeToken());
            previousToken.OnReload();
        }
    }
}
