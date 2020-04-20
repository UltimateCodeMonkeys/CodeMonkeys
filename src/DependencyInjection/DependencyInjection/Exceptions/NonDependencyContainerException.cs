using System;
using System.Runtime.Serialization;

namespace CodeMonkeys.DependencyInjection
{
    internal class NonDependencyContainerException :
        Exception
    {
        internal NonDependencyContainerException(
            Type type)
            : this($"The type {type} does not implement {typeof(DependencyContainer)}!")
        { }

        internal NonDependencyContainerException(
            Type type,
            Exception innerException)
            : this(
                  $"The type {type} does not implement {typeof(DependencyContainer)}!",
                  innerException)
        { }

        private NonDependencyContainerException(
            string message)
            : base(message)
        { }

        private NonDependencyContainerException(
            string message,
            Exception innerException)
            : base(message, innerException)
        { }

        protected NonDependencyContainerException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context)
        { }
    }
}