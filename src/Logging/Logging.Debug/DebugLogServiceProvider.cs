namespace CodeMonkeys.Logging.Debug
{
    public sealed class DebugServiceProvider : LogServiceProvider<DebugLogOptions>
    {
        public override ILogService Create(string context)
        {
            Argument.NotEmptyOrWhiteSpace(
                context,
                nameof(context));

            return new DebugLogService(this, context);
        }
    }
}
