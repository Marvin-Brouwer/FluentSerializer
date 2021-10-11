using FluentSerializer.Core.Context;
using FluentSerializer.Core.NamingStrategies;
using System;
using System.Reflection;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Profiles
{
    internal class ResponseTypeNamingStrategy : INamingStrategy
    {
        public string GetName(PropertyInfo property, INamingContext namingContext)
        {
            var genericTargetType = property.PropertyType.IsGenericType
                   ? property.PropertyType.GetTypeInfo().GenericTypeArguments[0]
                   : property.PropertyType;

            var itemNamingStrategy = namingContext.FindNamingStrategy(genericTargetType)
                ?? throw new NotSupportedException("Cannot support a type that is has no registered namingstrategy");

            return itemNamingStrategy.GetName(property, namingContext);
        }

        public string GetName(Type classType, INamingContext namingContext)
        {
            throw new NotSupportedException("This converter is meant for properties only.");
        }
    }
}