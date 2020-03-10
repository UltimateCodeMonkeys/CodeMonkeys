using System;

namespace CodeMonkeys.ExceptionHandling.Exceptions
{
    [Serializable]
    public class NotRegisteredException : Exception
    {
        public NotRegisteredException(
            Type affectedType)
            : this($"The type '{affectedType}' is not registered and can thus not be used in this context!")
        { }

        public NotRegisteredException(
            Type affectedType,
            Exception innerException)
            : this(
                  $"The type '{affectedType}' is not registered and can thus not be used in this context!",
                  innerException)
        { }

        private NotRegisteredException(
            string message)
            : base(message)
        { }

        private NotRegisteredException(
            string message,
            Exception innerException)
            : base(message, innerException)
        { }

        protected NotRegisteredException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }
    }
}