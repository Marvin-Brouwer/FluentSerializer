using FluentSerializer.Core.Context;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.UseCase.Mavenlink.Models;

using System;
using System.Reflection;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.NamingStrategies;

/// <summary>
/// Get's the name of the current request's data type
/// </summary>
internal class RequestEntityNamingStrategy : INamingStrategy
{
	public ReadOnlySpan<char> GetName(in PropertyInfo propertyInfo, in Type propertyType, in INamingContext namingContext)
	{
	    var typeName = propertyType.Name;
	    return EntityMappings.GetDataItemName(in typeName);
	}

	public ReadOnlySpan<char> GetName(in Type classType, in INamingContext namingContext)
	{
		throw new NotSupportedException("This converter is meant for properties only.");
	}
}