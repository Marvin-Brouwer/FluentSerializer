using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Naming.NamingStrategies;

using System;
using System.Reflection;

namespace FluentSerializer.UseCase.OpenAir.Serializer.NamingStrategies;

/// <summary>
/// Get's the name of the current response's data type
/// </summary>
internal sealed class ResponseTypeNamingStrategy : INamingStrategy
{
	public ReadOnlySpan<char> GetName(in PropertyInfo propertyInfo, in Type propertyType, in INamingContext namingContext)
	{
		var genericTargetType = propertyType.IsGenericType
			? propertyType.GetTypeInfo().GenericTypeArguments[0]
			: propertyType;

		var itemNamingStrategy = namingContext.FindNamingStrategy(in genericTargetType)
								 ?? throw new NotSupportedException("Cannot support a type that is has no registered naming strategy");

		return itemNamingStrategy.SafeGetName(propertyInfo, propertyType, namingContext);
	}

	public ReadOnlySpan<char> GetName(in Type classType, in INamingContext namingContext)
	{
		throw new NotSupportedException("This converter is meant for properties only.");
	}
}