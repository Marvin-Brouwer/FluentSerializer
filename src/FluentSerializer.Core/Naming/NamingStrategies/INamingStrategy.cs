using System;
using System.Reflection;
using FluentSerializer.Core.Context;

namespace FluentSerializer.Core.Naming.NamingStrategies;

public interface INamingStrategy
{
	public string GetName(PropertyInfo property, INamingContext namingContext);
	public string GetName(Type classType, INamingContext namingContext);
}