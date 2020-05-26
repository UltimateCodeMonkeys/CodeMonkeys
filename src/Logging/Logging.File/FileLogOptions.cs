using CodeMonkeys.Logging.Batching;

using System;

namespace CodeMonkeys.Logging.File
{
    public class FileLogOptions : BatchingLogOptions
    {
        /// <summary>
        /// Contains the file name prefix to use for log files.
        /// <para>Defaults to '_log'</para>
        /// <para>The value at time of attaching to the provider is used. This value is not monitored further.</para>
        /// </summary>
        public string FileNamePrefix
        {
            get => GetValue<string>();
            set
            {
                Property.NotEmptyOrWhiteSpace(value);
                SetValue(value);
            }
        }

        /// <summary>
        /// File extension of <see cref="FileNamePrefix"/>.
        /// <para>Defaults to 'txt'</para>
        /// <para>The value at time of attaching to the provider is used. This value is not monitored further.</para>
        /// </summary>
        public string Extension
        {
            get => GetValue<string>();
            set
            {
                Property.NotEmptyOrWhiteSpace(value);
                SetValue(value?.TrimStart('.'));
            }
        }

        /// <summary>
        /// The max size of a log file in bytes or <see langword="null"/> for no limit.
        /// <para>Defaults to <see langword="null"/></para>
        /// <para>The value at time of attaching to the provider is used. This value is not monitored further.</para>
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
        /// <para>Defaults to <see langword="null"/></para>
        /// <para>The value at time of attaching to the provider is used. This value is not monitored further.</para>
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
        /// Contains the path to the directory where the log files should be stored
        /// <para>Defaults to 'CurrentDirectory\logs'</para>
        /// <para>The value at time of attaching to the provider is used. This value is not monitored further.</para>
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

        public FileLogOptions()
        {
            FileNamePrefix = $"log-{DateTime.Now.ToShortDateString()}";
            Extension = "txt";
            Directory = $"{Environment.CurrentDirectory}\\logs";
        }
    }
}
