using CodeMonkeys.Logging.Batching;

using System;

namespace CodeMonkeys.Logging.File
{
    public class FileLogOptions : BatchLogOptions
    {
        private readonly string DEFAULT_FILENAMEPREFIX = $"log-{DateTime.Now.ToShortDateString()}";
        private const string DEFAULT_EXTENSION = "txt";

        /// <summary>
        /// The file name prefix used when creating log files.
        /// <para>Default value: 'log-DateTime.Now.ToShortDateString'</para>
        /// </summary>
        public string FileNamePrefix
        {
            get => GetValue<string>(DEFAULT_FILENAMEPREFIX);
            set
            {
                Property.NotEmptyOrWhiteSpace(value);
                SetValue(value);
            }
        }

        /// <summary>
        /// The file extension of the log files.
        /// <para>Default value: 'txt'</para>
        /// </summary>
        public string Extension
        {
            get => GetValue<string>(DEFAULT_EXTENSION);
            set
            {
                Property.NotEmptyOrWhiteSpace(value);
                SetValue(value?.TrimStart('.'));
            }
        }

        /// <summary>
        /// The max size of a log file in bytes or <see langword="null"/> for no limit.
        /// <para>Default value: <see langword="null"/></para>
        /// </summary>
        public long? MaxFileSize
        {
            get => GetValue<long?>();
            set
            {
                if (value != null)
                    Property.GreaterThan(value.Value, 0);

                SetValue(value);
            }
        }

        /// <summary>
        /// The maximum retained file count or <see langword="null"/> for no limit.
        /// <para>Default value: <see langword="null"/></para>
        /// </summary>
        public int? MaxFilesToRetain
        {
            get => GetValue<int?>();
            set
            {
                if (value != null)
                    Property.GreaterThan(value.Value, 0);

                SetValue(value);
            }
        }

        /// <summary>
        /// The path to the directory where the log files should be stored.
        /// </summary>
        public string Directory
        {
            get => GetValue<string>();
            set
            {
                Property.NotEmptyOrWhiteSpace(value);
                SetValue(value);
            }
        }
    }
}
