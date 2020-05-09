using Microsoft.Extensions.Primitives;

using System;

namespace CodeMonkeys.Configuration
{
    public abstract class OptionsConsumer<TOptions> : IDisposable
        where TOptions : Options
    {
        private bool disposed = false;

        protected IDisposable OptionsChangeToken;

        protected OptionsConsumer(TOptions options)
        {
            OptionsChangeToken = ChangeToken.OnChange(
                options.GetChangeToken,
                OnOptionsChanged,
                options);
        }        

        protected abstract void OnOptionsChanged(TOptions options);

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    OptionsChangeToken?.Dispose();

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
