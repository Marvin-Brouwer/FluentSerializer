using System;
using System.Reflection;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.UseCase.Mavenlink.Models;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.NamingStrategies;

/// <summary>
/// Get's the name of the reference group pointer for this reference
/// </summary>
internal class ReferenceGroupNamingStrategy : INamingStrategy
{
	public string GetName(in PropertyInfo property, in Type propertyType, in INamingContext namingContext)
	{
		string typeName = GetTypeName(propertyType);
		return EntityMappings.GetDataReferenceGroupName(in typeName);
	}

	public string GetName(in Type classType, in INamingContext namingContext)
	{
		return EntityMappings.GetDataReferenceGroupName(classType.Name);
	}

	private static string GetTypeName(Type type)
	{
		if (!type.IsEnumerable()) return type.Name;
		return type.GetGenericArguments()[0]!.Name;
	}
}