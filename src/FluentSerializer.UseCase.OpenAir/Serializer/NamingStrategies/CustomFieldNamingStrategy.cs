using FluentSerializer.Core.Context;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Naming.NamingStrategies;

using System;
using System.Reflection;

namespace FluentSerializer.UseCase.OpenAir.Serializer.NamingStrategies;

/// <summary>
/// Generates a custom field name by appending __c
/// </summary>
public sealed class CustomFieldNamingStrategy : INamingStrategy
{
	private readonly INamingStrategy _innerNamingStrategy;

	/// <summary>
	/// Generates a custom field name by appending __c to the <paramref name="name"/> given
	/// </summary>
	public CustomFieldNamingStrategy(in string name)
	{
		_innerNamingStrategy = Names.Equal(name)();
	}

	/// <summary>
	/// Generates a custom field name by appending __c to the property in snake_case
	/// </summary>
	public CustomFieldNamingStrategy()
	{
		_innerNamingStrategy = Names.Use.SnakeCase();
	}

	public ReadOnlySpan<char> GetName(in PropertyInfo propertyInfo, in Type propertyType, in INamingContext namingContext) => GetName(_innerNamingStrategy.GetName(in propertyInfo, in propertyType, in namingContext));
	public ReadOnlySpan<char> GetName(in Type classType, in INamingContext namingContext) => GetName(_innerNamingStrategy.GetName(in classType, in namingContext));

#if NET7_0_OR_GREATER
	private static ReadOnlySpan<char> GetName(ReadOnlySpan<char> name) => $"{name.ToString()}__c";
#else
	private static ReadOnlySpan<char> GetName(in ReadOnlySpan<char> name) => $"{name.ToString()}__c";
#endif
}