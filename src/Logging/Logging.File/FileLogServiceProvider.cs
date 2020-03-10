using CodeMonkeys.Core;
using CodeMonkeys.Core.Logging;
using CodeMonkeys.Logging.Batching;

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CodeMonkeys.Logging.File
{
    internal sealed class FileLogServiceProvider : BatchingLogServiceProvider<FileLogOptions>
    {
        private readonly string _fileName;
        private readonly string _extension;
        private readonly string _directory;

        private FileLogMessageFormatter _formatter;

        internal FileLogServiceProvider(FileLogOptions options) 
            : base(options)
        {
            _fileName = options.Name;
            _extension = options.Extension;
            _directory = options.Directory;
        }

        internal new void ProcessMessage(LogMessage message) => base.ProcessMessage(message);

        public override ILogService Create(string context)
        {
            Argument.NotEmptyOrWhiteSpace(
                context,
                nameof(context));

            return new FileLogService(this, context);
        }

        protected override async Task ProcessBatch(IEnumerable<LogMessage> batch, CancellationToken token)
        {
            _formatter ??= new FileLogMessageFormatter();

            Directory.CreateDirectory(_directory);

            foreach (var message in batch)
            {
                string path = Path.Combine(_directory, $"{_fileName}.{_extension}");
                string formattedMessage = _formatter.Format(message, TimeStampFormat);

                await FileAppendAllTextAsync(
                    path,
                    formattedMessage,
                    token);
            }
        }

        private async Task FileAppendAllTextAsync(
            string path, 
            string content,
            CancellationToken token)
        {
            await System.IO.File.AppendAllTextAsync(
                path,
                content,
                token);
        }
    }
}
