using System;
using System.Reflection;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.Core.Extensions;

public static class NamingExtensions
{
	public static string SafeGetName(this INamingStrategy namingStrategy, PropertyInfo property, INamingContext namingContext)
	{
		var resolvedName = namingStrategy.GetName(property, namingContext);
		Guard.Against.InvalidName(resolvedName, nameof(resolvedName));

		return resolvedName;
	}

	public static string SafeGetName(this INamingStrategy namingStrategy, Type classType, INamingContext namingContext)
	{
		var resolvedName = namingStrategy.GetName(classType, namingContext);
		Guard.Against.InvalidName(resolvedName, nameof(resolvedName));

		return resolvedName;
	}

	public static void InvalidName(this IGuardClause guard, string? value, string name) {
            
		guard.NullOrWhiteSpace(value, name);
		guard.InvalidFormat(value, name, @"^[\w_\-+]*$");
	}
}