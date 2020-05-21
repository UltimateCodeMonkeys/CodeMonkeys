﻿using CodeMonkeys.Logging.Batching;

using System;

namespace CodeMonkeys.Logging.File
{
    public class FileLogOptions : BatchingLogOptions
    {
        private string _name;
        private string _extension;
        private long? _maxSize;
        private string _directory;

        /// <summary>
        /// Contains the filename prefix to use for log files.
        /// <para>Defaults to '_log'</para>
        /// <para>The value at time of attaching to the provider is used. This value is not monitored further.</para>
        /// </summary>
        public string FileName
        {
            get => _name;
            set
            {
                Property.NotEmptyOrWhiteSpace(value);
                SetValue(ref _name, value);
            }
        }

        /// <summary>
        /// File extension of <see cref="FileName"/>.
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
        /// The max size of a log file in bytes after which a new one will be created
        /// </summary>
        public long? MaxFileSize
        {
            get => _maxSize;
            set
            {
                Property.GreaterThan(value, 0);
                SetValue(ref _maxSize, value);
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
            FileName = $"log-{DateTime.Now.ToShortDateString()}";
            Extension = "txt";
            Directory = $"{Environment.CurrentDirectory}\\logs";
        }
    }
}
