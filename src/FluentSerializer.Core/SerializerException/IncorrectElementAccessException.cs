using System;

namespace FluentSerializer.Core.SerializerException
{
    public sealed class IncorrectElementAccessException : OperationNotSupportedException
    {
        public Type ClassType { get; }
        public string TargetName { get; }
        public string ElementName { get; }

        public IncorrectElementAccessException(Type classType, string targetName, string elementName) : base(
            $"The name used for '{classType.FullName}' ({targetName}) does not match current element name '{elementName}'")
        {
            ClassType = classType;
            TargetName = targetName;
            ElementName = elementName;
        }
    }
}
