using CodeMonkeys.Core.Interfaces.ExceptionHandling;

using System;
using System.Collections.Generic;

namespace CodeMonkeys.ExceptionHandling
{
    public class ExceptionHandler :
        IExceptionHandler
    {
        protected readonly IList<WeakReference<Action<Exception>>> _handleActionReferences;

        protected ExceptionHandler()
        {
            _handleActionReferences = new List<WeakReference<Action<Exception>>>();
        }

        public ExceptionHandler(
            Action<Exception> handler)
            
            : this()
        {
            AddHandler(handler);
        }

        public ExceptionHandler(
            IEnumerable<Action<Exception>> handlers)

            : this()
        {
            foreach (var handler in handlers)
            {
                AddHandler(handler);
            }
        }


        public virtual void Handle(
            Exception exception)
        {
            if (exception == null)
                return;

            foreach (var reference in _handleActionReferences)
            {
                if (!reference.TryGetTarget(out var handler))
                    continue;

                handler(exception);
            }
        }


        private void AddHandler(
            Action<Exception> handler)
        {
            if (handler == null)
                return;


            var weakReference = new WeakReference<Action<Exception>>(
            handler);

            _handleActionReferences.Add(
                weakReference);
        }
    }
}