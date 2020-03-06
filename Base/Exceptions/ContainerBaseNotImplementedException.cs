using System;
using System.Runtime.Serialization;

namespace CodeMonkeys.DependencyInjection.Core.Exceptions
{
    internal class ContainerBaseNotImplementedException :
        Exception
    {
        internal ContainerBaseNotImplementedException(
            Type type)
            : this($"The type {type} does not implement {typeof(DependencyContainerBase)}!")
        { }

        internal ContainerBaseNotImplementedException(
            Type type,
            Exception innerException)
            : this(
                  $"The type {type} does not implement {typeof(DependencyContainerBase)}!",
                  innerException)
        { }

        private ContainerBaseNotImplementedException(
            string message)
            : base(message)
        { }

        private ContainerBaseNotImplementedException(
            string message,
            Exception innerException)
            : base(message, innerException)
        { }

        protected ContainerBaseNotImplementedException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context)
        { }
    }
}