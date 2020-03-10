﻿using CodeMonkeys.Core.Interfaces.ExceptionHandling;
using CodeMonkeys.Core.Interfaces.Logging;
using CodeMonkeys.ExceptionHandling.Exceptions;

using System;
using System.Collections.Concurrent;

namespace CodeMonkeys.ExceptionHandling
{
    public class ExceptionService :
        IExceptionService
    {
        protected readonly ILogService _log;

        protected readonly ConcurrentDictionary<Type, WeakReference<IExceptionHandler>> _exceptionHandlers =
            new ConcurrentDictionary<Type, WeakReference<IExceptionHandler>>();


        public ExceptionService(
            ILogService logService)
        {
            _log = logService;
        }


        /// <inheritdoc />
        public virtual void Handle<TException>(
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
        public virtual void RegisterHandler<TException>(
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
        public virtual void RegisterHandler<TException>(
            Action<TException> handleAction)
            where TException : Exception
        {
            var exceptionAction = new Action<Exception>(
                aiException => handleAction((TException)aiException));

            var exceptionHandler = new ExceptionHandler(exceptionAction);

            RegisterHandler<TException>(exceptionHandler);
        }


        /// <inheritdoc />
        public virtual void UnregisterHandler<TException>()
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
        public virtual bool IsExceptionTypeRegistered<TException>()
            where TException : Exception
        {
            lock (_exceptionHandlers)
                return _exceptionHandlers.ContainsKey(
                    typeof(TException));
        }


        /// <inheritdoc />
        public virtual IExceptionHandler GetHandler<TException>()
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