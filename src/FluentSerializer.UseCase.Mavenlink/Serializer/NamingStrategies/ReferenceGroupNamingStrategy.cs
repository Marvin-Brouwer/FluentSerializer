using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.UseCase.Mavenlink.Models;

using System;
using System.Reflection;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.NamingStrategies;

/// <summary>
/// Get's the name of the reference group pointer for this reference
/// </summary>
internal class ReferenceGroupNamingStrategy : INamingStrategy
{
	public ReadOnlySpan<char> GetName(in PropertyInfo property, in Type propertyType, in INamingContext namingContext)
	{
		var typeName = GetTypeName(propertyType);
		return EntityMappings.GetDataReferenceGroupName(in typeName);
	}

	public ReadOnlySpan<char> GetName(in Type classType, in INamingContext namingContext)
	{
		return EntityMappings.GetDataReferenceGroupName(classType.Name);
	}

	private static string GetTypeName(Type type)
	{
		if (!type.IsEnumerable()) return type.Name;
		return type.GetGenericArguments()[0].Name;
	}
}