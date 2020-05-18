using CodeMonkeys.Logging.Batching;

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
        private readonly int? _maxFileSize;
        private readonly string _path;
        private int _index;

        private LogMessageFormatter _formatter;

        internal FileLogServiceProvider(FileLogOptions options) 
            : base(options)
        {
            _fileName = options.FileName;
            _extension = options.Extension;
            _maxFileSize = options.MaxFileSize;
            _path = options.Directory;
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
                    var path = GetFilePath();

                    string formattedMessage = _formatter.Format(message, TimeStampFormat);

                    await FileAppendAllTextAsync(
                        path,
                        formattedMessage,
                        token);
                }
                catch { }
            }
        }

        private string GetFilePath()
        {
            var path = GetLogFileFullPath();
            var file = new FileInfo(path);

            if (_maxFileSize > 0 && file.Exists && file.Length < _maxFileSize)
                return path;

            _index++;
            return GetLogFileFullPath();
        }

        private string GetLogFileFullPath()
        {
            var suffix = _index == 0 ?
                 string.Empty :
                 $"({_index})";

            return Path.Combine(
                _path,
                $"{_fileName}{suffix}.{_extension}");
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
