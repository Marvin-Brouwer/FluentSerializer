using System;
using System.Collections.Generic;
using System.Reflection;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.UseCase.Mavenlink.Models;

namespace FluentSerializer.UseCase.Mavenlink.Serializer.NamingStrategies
{
	/// <summary>
	/// Get's the name of the current request's data type
	/// </summary>
    internal class RequestEntityNameStrategy : INamingStrategy
	{
		private static readonly Dictionary<string, string> EntityMappings = new()
		{
			[nameof(Project)] = "workspace",
			[nameof(User)] = "user"
		};

        public string GetName(in PropertyInfo property, in Type propertyType, in INamingContext namingContext)
        {
	        var typeName = propertyType.Name;
	        if (!EntityMappings.TryGetValue(typeName, out var propertyName))
		        throw new ArgumentException($"No entity mapping for entity {typeName} exists");

	        return propertyName;
        }

        public string GetName(in Type classType, in INamingContext namingContext)
        {
            throw new NotSupportedException("This converter is meant for properties only.");
        }
    }
}