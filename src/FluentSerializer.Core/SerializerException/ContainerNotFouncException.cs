using System;

namespace FluentSerializer.Core.SerializerException
{
    public sealed class ContainerNotFouncException : OperationNotSupportedException
    {
        public Type PropertyType { get; }
        public Type ContainerType { get; }
        public string TargetName { get; }

        public ContainerNotFouncException(Type propertyType, Type containerType, string targetName) : base(
            $"Container '{targetName}' of type '{containerType.Name}' was not found and '{targetName}' is not nullable")
        {
            PropertyType = propertyType;
            ContainerType = containerType;
            TargetName = targetName;
        }
    }
}
