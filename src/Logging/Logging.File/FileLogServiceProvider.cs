using CodeMonkeys.Logging.Batching;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SystemIOFile = System.IO.File;

namespace CodeMonkeys.Logging.File
{
    public sealed class FileLogServiceProvider : BatchingLogServiceProvider<FileLogOptions>
    {
        private readonly LogMessageFormatter _formatter;

        private int _index;

        internal FileLogServiceProvider()
        {
            _formatter = new LogMessageFormatter();
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
            Directory.CreateDirectory(Options.Directory);

            foreach (var message in batch)
            {
                try
                {
                    var path = CreateLogFilePath();

                    string formattedMessage = _formatter.Format(message, Options.TimeStampFormat);

                    await SystemIOFile.AppendAllTextAsync(
                        path,
                        formattedMessage,
                        token);
                }
                catch { }
            }

            ClearObsoleteFiles();
        }

        private string CreateLogFilePath()
        {
            var path = GetFullLogFilePath();

            if (Options.MaxFileSize == null)
                return path;

            var size = GetFileSize(path);

            if (size <= Options.MaxFileSize)
                return path;

            _index++;
            return GetFullLogFilePath();
        }

        private string GetFullLogFilePath()
        {
            var suffix = GetFileNameSuffix();

            return Path.Combine(
                Options.Directory,
                $"{Options.FileNamePrefix}{suffix}.{Options.Extension}");
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
                 $"-{_index}";

            return suffix;
        }

        private void ClearObsoleteFiles()
        {
            if (Options.MaxFilesToRetain == null)
                return;

            var filesToRemove = new DirectoryInfo(Options.Directory)
                .GetFiles(Options.FileNamePrefix + "*." + Options.Extension)
                .OrderByDescending(f => f.LastWriteTime)
                .Skip(Options.MaxFilesToRetain.Value);

            foreach (var file in filesToRemove)
                file.Delete();
        }
    }
}
