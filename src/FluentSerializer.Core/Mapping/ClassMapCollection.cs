using Ardalis.GuardClauses;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Extensions;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FluentSerializer.Core.Mapping;

/// <inheritdoc />
[StructLayout(LayoutKind.Sequential, Pack = 0)]
public readonly struct ClassMapCollection : IClassMapCollection
{
	private readonly IReadOnlyCollection<IClassMap> _classMaps;

	/// <inheritdoc cref="IClassMapCollection" />
	public ClassMapCollection(in IReadOnlyCollection<IClassMap> classMaps)
	{
		_classMaps = classMaps;
	}

	/// <inheritdoc />
	public IClassMap? GetClassMapFor(in Type type, in SerializerDirection direction)
	{
		Guard.Against.Null(type
#if NETSTANDARD
			, nameof(type)
#endif
		);
		Guard.Against.InvalidChoice(
			direction, SerializerDirection.Both,
			$"You cannot get a {nameof(ClassMap)} for {nameof(SerializerDirection)}.{SerializerDirection.Both} \n" +
			"you can only register one as such!"
		);

		foreach (var classMap in _classMaps)
		{
			if (!MatchDirection(in direction, classMap.Direction)) continue;
			if (!MatchType(classMap.ClassType, in type)) continue;

			return classMap;
		}

		return default;
	}

	/// <inheritdoc />
	public IClassMap? GetClassMapFor(in Type type)
	{
		Guard.Against.Null(type
#if NETSTANDARD
			, nameof(type)
#endif
		);

		foreach (var classMap in _classMaps)
		{
			if (!MatchType(classMap.ClassType, in type)) continue;

			return classMap;
		}

		return default;
	}

	private static bool MatchDirection(in SerializerDirection searchDirection, in SerializerDirection mapDirection)
	{
		if (mapDirection == SerializerDirection.Both) return true;

		return searchDirection == mapDirection;
	}

	private static bool MatchType(in Type classType, in Type type)
	{
		var concreteCompareTo = Nullable.GetUnderlyingType(type) ?? type;
		return concreteCompareTo.EqualsTopLevel(classType);
	}
}
