using System;
using System.Reflection;
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.Core.Context;

/// <summary>
/// Current context for resolving names of types
/// </summary>
public interface INamingContext
{
	/// <summary>
	/// Find the <see cref="INamingStrategy"/>  for any property of a certain <see cref="Type"/> if registered
	/// This can be useful when unpacking collections to a different data structure
	/// </summary>
	INamingStrategy? FindNamingStrategy(in Type classType, in PropertyInfo property);

	/// <summary>
	/// Find the <see cref="INamingStrategy"/>  for any <see cref="Type"/> if registered
	/// This can be useful when unpacking collections to a different data structure
	/// </summary>
	INamingStrategy? FindNamingStrategy(in Type type);
}