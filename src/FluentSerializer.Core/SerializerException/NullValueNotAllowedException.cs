using System;

namespace FluentSerializer.Core.SerializerException
{
    public sealed class NullValueNotAllowedException : OperationNotSupportedException
    {
        public Type PropertyType { get; }
        public string TargetName { get; }

        public NullValueNotAllowedException(Type propertyType, string targetName) : base(
            $"Value of '{targetName}' evaluated to null, which is not allowed for '{propertyType.FullName}'")
        {
            PropertyType = propertyType;
            TargetName = targetName;
        }
    }
}
