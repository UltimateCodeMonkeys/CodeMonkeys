using CodeMonkeys.Logging.Batching;

namespace CodeMonkeys.Logging.File
{
    public class FileLogOptions : BatchingLogOptions
    {
        private int? _maxFileSize;

        private string _fileNamePrefix;
        private string _fileExtension;
        private string _directory;

        public int? MaxFileSize
        {
            get => _maxFileSize;
            set => SetValue(ref _maxFileSize, value);
        }

        public string FileNamePrefix
        {
            get => _fileNamePrefix;
            set => SetValue(ref _fileNamePrefix, value?.TrimStart('.'));
        }

        public string FileExtension
        {
            get => _fileExtension;
            set => SetValue(ref _fileExtension, value);
        }

        public string Directory
        {
            get => _directory;
            set => SetValue(ref _directory, value);
        }

        public FileLogOptions()
        {
            MaxFileSize = 10 * 1024 * 1024;
            FileNamePrefix = "logs_";
            FileExtension = "txt";
            Directory = "logs";
        }
    }
}
