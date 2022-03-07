using System;
using System.Reflection;
using FluentSerializer.Core.Context;
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
	    var typeName = propertyType.Name;
	    return EntityMappings.GetDataReferenceGroupName(in typeName);
	}

	public string GetName(in Type classType, in INamingContext namingContext)
	{
		return EntityMappings.GetDataReferenceGroupName(classType.Name);
	}
}