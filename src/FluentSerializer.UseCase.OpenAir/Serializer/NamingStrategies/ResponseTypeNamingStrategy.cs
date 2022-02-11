using System;
using System.Reflection;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.UseCase.OpenAir.Serializer.NamingStrategies
{
	/// <summary>
	/// Get's the name of the current response's data type
	/// </summary>
    internal class ResponseTypeNamingStrategy : INamingStrategy
    {
        public string GetName(in PropertyInfo property, in INamingContext namingContext)
        {
            var genericTargetType = property.PropertyType.IsGenericType
                   ? property.PropertyType.GetTypeInfo().GenericTypeArguments[0]
                   : property.PropertyType;

            var itemNamingStrategy = namingContext.FindNamingStrategy(in genericTargetType)
                ?? throw new NotSupportedException("Cannot support a type that is has no registered naming strategy");

            return itemNamingStrategy.SafeGetName(property, namingContext);
        }

        public string GetName(in Type classType, in INamingContext namingContext)
        {
            throw new NotSupportedException("This converter is meant for properties only.");
        }
    }
}