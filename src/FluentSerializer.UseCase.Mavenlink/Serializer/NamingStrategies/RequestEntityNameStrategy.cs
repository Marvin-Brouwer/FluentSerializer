using System;
using System.Reflection;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.UseCase.Mavenlink.Models;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.NamingStrategies;

/// <summary>
/// Get's the name of the current request's data type
/// </summary>
internal class RequestEntityNameStrategy : INamingStrategy
{
	public string GetName(in PropertyInfo property, in Type propertyType, in INamingContext namingContext)
	{
	    var typeName = propertyType.Name;
	    return EntityMappings.GetDataItemName(in typeName);
	}

	public string GetName(in Type classType, in INamingContext namingContext)
	{
		throw new NotSupportedException("This converter is meant for properties only.");
	}
}