using CodeMonkeys.Logging.Batching;

namespace CodeMonkeys.Logging.File
{
    public class FileLogOptions : BatchingLogOptions
    {
        private string _name;
        private string _extension;

        /// <summary>
        /// Name of the file in which the log messages should be written.
        /// <para>Defaults to 'log'</para>
        /// </summary>
        public string FileName
        {
            get => _name;
            set => SetValue(ref _name, value);
        }

        /// <summary>
        /// File extension of <see cref="FileName"/>.
        /// <para>Defaults to 'txt'</para>
        /// </summary>
        public string Extension
        {
            get => _extension;
            set => SetValue(ref _extension, value?.TrimStart('.'));
        }

        public FileLogOptions()
        {
            FileName = "log";
            Extension = "txt";
        }
    }
}
