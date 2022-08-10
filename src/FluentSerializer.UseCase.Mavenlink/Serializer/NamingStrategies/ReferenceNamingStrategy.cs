using FluentSerializer.Core.Context;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.UseCase.Mavenlink.Models;

using System;
using System.Reflection;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.NamingStrategies;

/// <summary>
/// Get's the name of the pointer for this reference
/// </summary>
internal class ReferenceNamingStrategy : INamingStrategy
{
	public ReadOnlySpan<char> GetName(in PropertyInfo property, in Type propertyType, in INamingContext namingContext)
	{
	    var typeName = propertyType.Name;
	    return EntityMappings.GetDataReferenceName(in typeName);
	}

	public ReadOnlySpan<char> GetName(in Type classType, in INamingContext namingContext)
	{
		return EntityMappings.GetDataReferenceName(classType.Name);
	}
}