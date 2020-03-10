using CodeMonkeys.Logging.Batching;

namespace CodeMonkeys.Logging.File
{
    public class FileLogOptions : BatchingLogOptions
    {
        private string _name;
        private string _extension;
        private string _directory;

        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }

        public string Extension
        {
            get => _extension;
            set => SetValue(ref _extension, value?.TrimStart('.'));
        }

        public string Directory
        {
            get => _directory;
            set => SetValue(ref _directory, value);
        }

        public FileLogOptions()
        {
            Name = "log";
            Extension = "txt";
            Directory = "logs";
        }
    }
}
