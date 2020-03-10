using CodeMonkeys.Logging.Batching;

namespace CodeMonkeys.Logging.File
{
    public class FileLogOptions : BatchingLogOptions
    {
        private string _directory;

        public string Directory
        {
            get => _directory;
            set => SetValue(ref _directory, value);
        }
    }
}
