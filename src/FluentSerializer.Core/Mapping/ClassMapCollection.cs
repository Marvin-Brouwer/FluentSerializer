using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Extensions;

using System;
using System.Collections.Generic;

namespace FluentSerializer.Core.Mapping;

public readonly struct ClassMapCollection : IClassMapCollection
{
	private readonly IReadOnlyList<INewClassMap> _classMaps;

	public ClassMapCollection(IReadOnlyList<INewClassMap> classMaps)
	{
		_classMaps = classMaps;
	}

	public IReadOnlyList<INewClassMap> GetAllClassMaps() => _classMaps;

	public INewClassMap? GetClassMapFor(in Type type, in SerializerDirection direction)
	{
		// todo cache?
		if (direction == SerializerDirection.Both)
			throw new NotSupportedException(
				$"You cannot get a {nameof(ClassMap)} for {nameof(SerializerDirection)}.{SerializerDirection.Both} \n" +
				"you can only register one as such!");

		foreach (var classMap in _classMaps)
		{
			if (!MatchDirection(in direction, classMap.Direction)) continue;
			if (!MatchType(classMap.ClassType, in type)) continue;

			return classMap;
		}

		return default;
	}

	private static bool MatchDirection(in SerializerDirection searchDirection, in SerializerDirection mapDirection)
	{
		if (searchDirection == SerializerDirection.Both) return true;
		if (mapDirection == SerializerDirection.Both) return true;

		return searchDirection == mapDirection;
	}

	private static bool MatchType(in Type classType, in Type type)
	{
		var concreteCompareTo = Nullable.GetUnderlyingType(type) ?? type;
		return concreteCompareTo.EqualsTopLevel(classType);
	}
}
