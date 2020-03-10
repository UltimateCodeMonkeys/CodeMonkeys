﻿using CodeMonkeys.Core;
using CodeMonkeys.Core.Logging;

namespace CodeMonkeys.Logging.Console
{
    internal sealed class ConsoleLogServiceProvider : LogServiceProvider<ConsoleLogOptions>
    {
        private ConsoleLogMessageFormatter _formatter;
        private readonly bool _useColoredOutput;

        internal ConsoleLogServiceProvider(ConsoleLogOptions options)
            : base(options)
        {
            _useColoredOutput = options.UseColoredOutput;
        }        

        public override ILogService Create(string context)
        {
            Argument.NotEmptyOrWhiteSpace(
                context,
                nameof(context));
            
            return new ConsoleLogService(this, context);
        }

        public override void ProcessMessage(LogMessage message)
        {
            _formatter ??= new ConsoleLogMessageFormatter(
                _useColoredOutput);

            ConsoleWriteLine(_formatter
                .Format(
                    message, 
                    TimeStampFormat));
        }

        private void ConsoleWriteLine(string value)
        {
            System.Console.WriteLine(value);
        }
    }
}