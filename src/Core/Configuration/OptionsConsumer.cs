using Microsoft.Extensions.Primitives;

using System;

namespace CodeMonkeys.Core.Configuration
{
    public abstract class OptionsConsumer<TOptions>
        where TOptions : Options
    {
        protected IDisposable OptionsChangeToken;

        protected OptionsConsumer(TOptions options)
        {
            OptionsChangeToken = ChangeToken.OnChange(
                options.GetChangeToken,
                OnOptionsHasChanged,
                options);
        }

        protected abstract void OnOptionsHasChanged(TOptions options);
    }
}
