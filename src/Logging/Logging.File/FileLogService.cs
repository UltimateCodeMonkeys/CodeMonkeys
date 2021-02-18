using CodeMonkeys.Logging.Batching;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        protected override async Task PublishMessageBatch(
            IEnumerable<LogMessage> messageBatch)
        {
            var directory = Directory.CreateDirectory(Options.Directory);

            if (!directory.Exists)
            {
                throw new DirectoryNotFoundException(
                    $"Log file directory creation failed! Maybe your application doesn't have write access to it?");
            }

            foreach (var message in messageBatch)
            {
                var path = GetLogFilePath();

                using (var stream = new FileStream(path, FileMode.Append))
                {
                    var bytes = Encoding.UTF8.GetBytes(
                        MessageFormatter.Format(
                            message,
                            Options.TimeStampFormat));

                    await stream.WriteAsync(bytes);
                }
            }

            RemoveDeprecatedFiles();
        }

        private string GetLogFilePath()
        {
            var path = GenerateLogFilePath();

            if (Options.MaxFileSize == null)
            {
                return path;
            }

            var size = GetFileSize(path);

            if (size <= Options.MaxFileSize)
            {
                return path;
            }

            return GenerateLogFilePath(true);
        }

        private string GenerateLogFilePath(
            bool increaseIndex = false)
        {
            if (increaseIndex)
            {
                _index++;
            }

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

        private void RemoveDeprecatedFiles()
        {
            if (Options.MaxFilesToRetain == null)
            {
                return;
            }

            var deprecatedFiles = GetDeprecatedFiles();

            foreach (var file in deprecatedFiles)
            {
                file.Delete();
            }
        }

        private IEnumerable<FileInfo> GetDeprecatedFiles()
        {
            var deprecatedFiles = new DirectoryInfo(Options.Directory)
                .GetFiles($"{Options.FileNamePrefix}*.{Options.Extension}")
                .OrderByDescending(f => f.LastWriteTime)
                .Skip(Options.MaxFilesToRetain.Value);

            return deprecatedFiles;
        }
    }
}
