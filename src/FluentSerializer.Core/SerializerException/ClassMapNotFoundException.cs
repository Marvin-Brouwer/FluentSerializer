using System;

namespace FluentSerializer.Core.SerializerException
{
    public sealed class ClassMapNotFoundException : SerializerException
    {
        public Type TargetType { get; }

        public ClassMapNotFoundException(Type targetType) : base(
            $"No ClassMap found for '{targetType.FullName}' \n" +
            "Make sure you've created a profile for it.")
        {
            TargetType = targetType;
        }
    }
}
