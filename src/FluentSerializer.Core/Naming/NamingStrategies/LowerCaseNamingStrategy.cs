using System;
using System.Reflection;
using FluentSerializer.Core.Context;

namespace FluentSerializer.Core.Naming.NamingStrategies;

public class LowerCaseNamingStrategy : INamingStrategy
{
	public string GetName(PropertyInfo property, INamingContext _) => property.Name.Split('`')[0].ToLowerInvariant();
	public string GetName(Type classType, INamingContext _) => classType.Name.Split('`')[0].ToLowerInvariant();
}