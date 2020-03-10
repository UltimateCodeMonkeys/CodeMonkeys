using CodeMonkeys.Core.Interfaces.ExceptionHandling;
using CodeMonkeys.Core.Interfaces.Logging;

using System;
using System.Collections.Concurrent;

namespace ExceptionHandling
{
    public class ExceptionService :
        IExceptionService
    {
        private readonly ILogService _log;

        private readonly ConcurrentDictionary<Type, WeakReference<IExceptionHandler>> _exceptionHandlers =
            new ConcurrentDictionary<Type, WeakReference<IExceptionHandler>>();


        public ExceptionService(
            ILogService logService)
        {
            _log = logService;
        }


        /// <inheritdoc />
        public void Handle<TException>(
            TException exception)
            where TException : Exception
        {
            lock (_exceptionHandlers)
            {
                if (!IsExceptionTypeRegistered<TException>())
                    throw new NotRegisteredException(typeof(TException));

                var handlerReference = _exceptionHandlers[
                    typeof(TException)];

                if (!handlerReference.TryGetTarget(
                    out var handler))
                {
                    _log.Info($"Handler reference for exception type {typeof(TException)} has been removed --- throwing");
                    throw exception;
                }

                handler.Handle(exception);
            }

            _log.Debug($"Finished handling exception of type {typeof(TException).Name}.");
        }


        /// <inheritdoc />
        public void RegisterHandler<TException>(
            IExceptionHandler handler)
            where TException : Exception
        {
            lock (_exceptionHandlers)
            {
                if (IsExceptionTypeRegistered<TException>())
                    return;

                _exceptionHandlers.TryAdd(
                    typeof(TException),
                    new WeakReference<IExceptionHandler>(
                        handler));
            }

            _log.Debug($"New handler registered for exception type {typeof(TException).Name}.");
        }


        /// <inheritdoc />
        public void RegisterHandler<TException>(
            Action<TException> handleAction)
            where TException : Exception
        {
            var exceptionAction = new Action<Exception>(
                aiException => handleAction((TException)aiException));

            var exceptionHandler = new ExceptionHandler(exceptionAction);

            RegisterHandler<TException>(exceptionHandler);
        }


        /// <inheritdoc />
        public void UnregisterHandler<TException>()
            where TException : Exception
        {
            lock (_exceptionHandlers)
            {
                if (!IsExceptionTypeRegistered<TException>())
                    return;

                _exceptionHandlers.TryRemove(
                    typeof(TException),
                    out _);
            }

            _log.Debug($"Handler registration for exception type {typeof(TException).Name} has been removed!");
        }


        /// <inheritdoc />
        public bool IsExceptionTypeRegistered<TException>()
            where TException : Exception
        {
            lock (_exceptionHandlers)
                return _exceptionHandlers.ContainsKey(
                    typeof(TException));
        }


        /// <inheritdoc />
        public IExceptionHandler GetHandler<TException>()
            where TException : Exception
        {
            lock (_exceptionHandlers)
            {
                if (!IsExceptionTypeRegistered<TException>())
                    throw new NotRegisteredException(typeof(TException));

                var handlerReference = _exceptionHandlers[
                    typeof(TException)];

                if (!handlerReference.TryGetTarget(
                    out var handler))
                {
                    _log.Info($"Handler reference for exception type {typeof(TException)} has been removed --- returning null");
                    return null;
                }

                return handler;
            }
        }
    }
}