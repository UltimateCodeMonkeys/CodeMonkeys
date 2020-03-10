using System;

namespace CodeMonkeys.Core.Interfaces.ExceptionHandling
{
    public interface IExceptionHandler
    {
        /// <summary>
        /// Gets invoked by the exception service when an exception of this ExceptionHandler's type occurs
        /// </summary>
        /// <param name="exception">The <see cref="System.Exception"> to handle</param>
        void Handle(
            Exception exception);
    }
}