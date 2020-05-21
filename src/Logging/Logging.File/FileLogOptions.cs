using CodeMonkeys.Logging.Batching;

using System;

namespace CodeMonkeys.Logging.File
{
    public class FileLogOptions : BatchingLogOptions
    {
        private string _fileNamePrefix;
        private string _extension;
        private long? _maxSize;
        private int? _maxFilesToRetain;
        private string _directory;

        /// <summary>
        /// Contains the file name prefix to use for log files.
        /// <para>Defaults to '_log'</para>
        /// <para>The value at time of attaching to the provider is used. This value is not monitored further.</para>
        /// </summary>
        public string FileNamePrefix
        {
            get => _fileNamePrefix;
            set
            {
                Property.NotEmptyOrWhiteSpace(value);
                SetValue(ref _fileNamePrefix, value);
            }
        }

        /// <summary>
        /// File extension of <see cref="FileNamePrefix"/>.
        /// <para>Defaults to 'txt'</para>
        /// <para>The value at time of attaching to the provider is used. This value is not monitored further.</para>
        /// </summary>
        public string Extension
        {
            get => _extension;
            set
            {
                Property.NotEmptyOrWhiteSpace(value);
                SetValue(ref _extension, value?.TrimStart('.'));
            }
        }

        /// <summary>
        /// The max size of a log file in bytes or <see langword="null"/> for no limit.
        /// <para>Defaults to <see langword="null"/></para>
        /// <para>The value at time of attaching to the provider is used. This value is not monitored further.</para>
        /// </summary>
        public long? MaxFileSize
        {
            get => _maxSize;
            set
            {
                if (value != null)
                    Property.GreaterThan(value.Value, 0);

                SetValue(ref _maxSize, value);
            }
        }

        /// <summary>
        /// The maximum retained file count or <see langword="null"/> for no limit.
        /// <para>Defaults to <see langword="null"/></para>
        /// <para>The value at time of attaching to the provider is used. This value is not monitored further.</para>
        /// </summary>
        public int? MaxFilesToRetain
        {
            get => _maxFilesToRetain;
            set
            {
                if (value != null)
                    Property.GreaterThan(value.Value, 0);

                SetValue(ref _maxFilesToRetain, value);
            }
        }

        /// <summary>
        /// Contains the path to the directory where the log files should be stored
        /// <para>Defaults to 'CurrentDirectory\logs'</para>
        /// <para>The value at time of attaching to the provider is used. This value is not monitored further.</para>
        /// </summary>
        public string Directory
        {
            get => _directory;
            set
            {
                Property.NotEmptyOrWhiteSpace(value);
                SetValue(ref _directory, value);
            }
        }

        public FileLogOptions()
        {
            FileNamePrefix = $"log-{DateTime.Now.ToShortDateString()}";
            Extension = "txt";
            Directory = $"{Environment.CurrentDirectory}\\logs";
        }
    }
}
