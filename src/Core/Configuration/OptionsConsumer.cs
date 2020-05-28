namespace CodeMonkeys.Configuration
{
    public abstract class OptionsConsumer<TOptions>
        where TOptions : Options, new()
    {
        public static readonly TOptions Options =
            new TOptions();
    }
}
