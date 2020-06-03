namespace CodeMonkeys.Configuration
{
    public class OptionsConsumer<TOptions>
        where TOptions : Options, new()
    {
        public static readonly TOptions Options =
            new TOptions();

        protected OptionsConsumer()
        {
        }
    }
}
