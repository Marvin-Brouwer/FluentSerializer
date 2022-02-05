using System;
using System.Reflection;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Naming.NamingStrategies;
using System.Runtime.CompilerServices;

namespace FluentSerializer.Core.Extensions;

/// <summary>
/// Extensions for safely handeling property/entity names
/// </summary>
public static class NamingExtensions
{
	/// <summary>
	/// Get the converted name for this <paramref name="property"/> using the applied <paramref name="namingStrategy"/>
	/// and validating the name
	/// </summary>
	public static string SafeGetName(this INamingStrategy namingStrategy, PropertyInfo property, INamingContext namingContext)
	{
		var resolvedName = namingStrategy.GetName(property, namingContext);
		Guard.Against.InvalidName(resolvedName);

		return resolvedName;
	}

	/// <summary>
	/// Get the converted name for this <paramref name="classType"/> using the applied <paramref name="namingStrategy"/>
	/// and validating the name
	/// </summary>
	public static string SafeGetName(this INamingStrategy namingStrategy, Type classType, INamingContext namingContext)
	{
		var resolvedName = namingStrategy.GetName(classType, namingContext);
		Guard.Against.InvalidName(resolvedName);

		return resolvedName;
	}

	/// <summary>
	/// Make sure the <paramref name="value"/> passed contains only valid characters.
	/// </summary>
	public static void InvalidName(this IGuardClause guard, string? value, [CallerArgumentExpression("value")] string name = "") {

		guard.NullOrWhiteSpace(value, name);
		guard.InvalidFormat(value, name, @"^[\w_\-+]*$");
	}
}