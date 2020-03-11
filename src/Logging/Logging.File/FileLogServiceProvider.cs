using CodeMonkeys.Core;
using CodeMonkeys.Core.Logging;
using CodeMonkeys.Logging.Batching;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CodeMonkeys.Logging.File
{
    public sealed class FileLogServiceProvider : BatchingLogServiceProvider<FileLogOptions>
    {
        private readonly string _fileName;
        private readonly string _extension;

        private LogMessageFormatter _formatter;

        internal FileLogServiceProvider(FileLogOptions options) 
            : base(options)
        {
            _fileName = options.FileName;
            _extension = options.Extension;
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
            _formatter ??= new LogMessageFormatter();

            foreach (var message in batch)
            {
                try
                {
                    string path = Path.Combine(Environment.CurrentDirectory, $"{_fileName}.{_extension}");
                    string formattedMessage = _formatter.Format(message, TimeStampFormat);

                    await FileAppendAllTextAsync(
                        path,
                        formattedMessage,
                        token);
                }
                catch { }
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
