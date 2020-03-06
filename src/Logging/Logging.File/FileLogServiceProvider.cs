using CodeMonkeys.Core;
using CodeMonkeys.Core.Logging;
using CodeMonkeys.Logging.Batching;

namespace CodeMonkeys.Logging.File
{
    public class FileLogServiceProvider : BatchingLogServiceProvider<FileLogOptions>
    {
        public FileLogServiceProvider(FileLogOptions options) 
            : base(options)
        {
        }

        internal new void EnqueueMessage(LogMessage message) => base.EnqueueMessage(message);

        public override ILogService Create(string context)
        {
            Argument.NotEmptyOrWhitespace(
                context,
                nameof(context));

            return new FileLogService(this, context);
        }
    }
}
