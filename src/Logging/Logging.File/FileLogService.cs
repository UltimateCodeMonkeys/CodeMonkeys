using CodeMonkeys.Logging.Batching;

using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeMonkeys.Logging.File
{
    /// <summary>
    /// <see cref="IScopedLogService"/> which writes to one or multiple <see cref="System.IO.File"/>('s).
    /// </summary>
    public sealed class FileLogService : BatchLogService<FileLogOptions>
    {
        private int _index;

        internal FileLogService(string context)
            : base(context)
        {
        }

        protected override async Task PublishMessageBatch(IEnumerable<LogMessage> messageBatch)
        {
            Directory.CreateDirectory(Options.Directory);

            foreach (var message in messageBatch)
            {
                try
                {
                    var path = CreateLogFilePath();

                    await System.IO.File.AppendAllTextAsync(
                        path,
                        MessageFormatter.Format(
                            message,
                            Options.TimeStampFormat));
                }
                catch { }
            }

            ClearObsoleteFiles();
        }

        private string CreateLogFilePath()
        {
            var path = GetFullLogFilePath();

            if (Options.MaxFileSize == null)
            {
                return path;
            }

            var size = GetFileSize(path);

            if (size <= Options.MaxFileSize)
            {
                return path;
            }

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
            {
                file.Delete();
            }
        }
    }
}
