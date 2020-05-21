using CodeMonkeys.Logging.Batching;

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SystemIOFile = System.IO.File;

namespace CodeMonkeys.Logging.File
{
    public sealed class FileLogServiceProvider : BatchingLogServiceProvider<FileLogOptions>
    {
        private readonly string _fileName;
        private readonly string _extension;
        private readonly long? _maxFileSize;
        private readonly string _directoryPath;
        private int _index;

        private LogMessageFormatter _formatter;

        private readonly string _path;

        internal FileLogServiceProvider(FileLogOptions options) 
            : base(options)
        {
            _fileName = options.FileName;
            _extension = options.Extension;
            _maxFileSize = options.MaxFileSize;
            _directoryPath = options.Directory;

            if (_maxFileSize == null)
            {
                _path = Path.Combine(
                    _directoryPath,
                    $"{_fileName}.{_extension}");
            }
        }

        internal new void ProcessMessage(LogMessage message) => 
            base.ProcessMessage(message);

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

            Directory.CreateDirectory(_directoryPath);

            foreach (var message in batch)
            {
                try
                {
                    var path = _maxFileSize == null ?
                        _path :
                        CreateLogFilePath();

                    string formattedMessage = _formatter.Format(message, TimeStampFormat);

                    await SystemIOFile.AppendAllTextAsync(
                        path,
                        formattedMessage,
                        token);
                }
                catch { }
            }
        }

        private string CreateLogFilePath()
        {
            var path = GetFullLogFilePath();
            var size = GetFileSize(path);

            if (size <= _maxFileSize)
                return path;

            _index++;
            return GetFullLogFilePath();
        }

        private string GetFullLogFilePath()
        {
            var suffix = GetFileNameSuffix();

            return Path.Combine(
                _directoryPath,
                $"{_fileName}{suffix}.{_extension}");
        }

        private long GetFileSize(string path)
        {
            var fi = new FileInfo(path);
            var size = fi.Exists ?
                fi.Length :
                0;

            return size;
        }

        private string GetFileNameSuffix()
        {
            var suffix = _index == 0 ?
                 string.Empty :
                 $"({_index})";

            return suffix;
        }
    }
}
