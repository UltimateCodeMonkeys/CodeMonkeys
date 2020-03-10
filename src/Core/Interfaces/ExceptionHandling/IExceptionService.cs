using System;

namespace CodeMonkeys.Core.Interfaces.ExceptionHandling
{
    public interface IExceptionService
    {
        /// <summary>
        /// Maps the exception type to the given action that should be invoked each time this exception occurs and <see cref="IExceptionHandler.Handle{TException}"/> gets called
        /// </summary>
        /// <typeparam name="TException">Type of the exception this action should be used for</typeparam>
        /// <param name="handleAction">The action to invoke</param>
        void RegisterHandler<TException>(
            Action<TException> handleAction)
            where TException : Exception;

        /// <summary>
        /// Maps the exception type to the given handler that should be informed each time this exception occurs and <see cref="IExceptionHandler.Handle{TException}"/> gets called
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="handler"></param>
        void RegisterHandler<TException>(
            IExceptionHandler handler)
            where TException : Exception;

        /// <summary>
        /// Removes a Handler/Exception registration
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        void UnregisterHandler<TException>()
            where TException : Exception;

        /// <summary>
        /// Checks wether a handler is registered for a given exception type
        /// </summary>
        /// <typeparam name="TException">Type of the exception to look for a handler</typeparam>
        /// <returns>True if a handler is registered; false if not</returns>
        bool IsExceptionTypeRegistered<TException>()
            where TException : Exception;

        /// <summary>
        /// Resolves the handler for the given exception type if it exists
        /// </summary>
        /// <typeparam name="TException">Exception type to resolve the handler for</typeparam>
        /// <returns>A handler instance if a registration has been found</returns>
        IExceptionHandler GetHandler<TException>()
            where TException : Exception;

        /// <summary>
        /// Looks up the <see cref="IExceptionHandler"/> for this exception type and invokes it to handle the exception
        /// </summary>
        /// <typeparam name="TException">Type of the exception</typeparam>
        /// <param name="exception">Exception that should be handled</param>
        void Handle<TException>(
            TException exception)
            where TException : Exception;
    }
}