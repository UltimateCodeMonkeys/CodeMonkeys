using CodeMonkeys.Core;
using CodeMonkeys.Core.Logging;
using CodeMonkeys.Logging.Batching;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CodeMonkeys.Logging.File
{
    public class FileLogServiceProvider : BatchingLogServiceProvider<FileLogOptions>
    {
        private readonly string _directory;

        public FileLogServiceProvider(FileLogOptions options) 
            : base(options)
        {
            _directory = options.Directory;
        }

        internal new void EnqueueMessage(LogMessage message) => base.EnqueueMessage(message);

        public override ILogService Create(string context)
        {
            Argument.NotEmptyOrWhiteSpace(
                context,
                nameof(context));

            return new FileLogService(this, context);
        }

        protected override async Task ProcessBatch(IEnumerable<LogMessage> batch, CancellationToken token)
        {
            Directory.CreateDirectory(_directory);
        }
    }
}
