using System;

namespace FluentSerializer.Core.SerializerException
{
    public sealed class ContainerNotSupportedException : OperationNotSupportedException
    {
        public Type ContainerType { get; }

        public ContainerNotSupportedException(Type containerType) : base(
            $"The serial container of type '{containerType.Name}' is not supported by this ISerializer")
        {
            ContainerType = containerType;
        }
    }
}
