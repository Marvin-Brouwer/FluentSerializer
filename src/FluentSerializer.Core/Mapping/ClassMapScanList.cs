using FluentSerializer.Core.Extensions;
using System;
using System.Collections.Generic;
using FluentSerializer.Core.Configuration;

namespace FluentSerializer.Core.Mapping;

public sealed class ClassMapScanList : ScanList<(Type type, SerializerDirection direction), IClassMap>
{
	public ClassMapScanList(IReadOnlyList<IClassMap> dataTypes) : base(dataTypes) { }

	protected override bool Compare((Type type, SerializerDirection direction) compareTo, IClassMap dataType)
	{
		if (!MatchDirection(compareTo.direction, dataType.Direction)) return false;

		var concreteCompareTo = Nullable.GetUnderlyingType(compareTo.type) ?? compareTo.type;
		return concreteCompareTo.EqualsTopLevel(dataType.ClassType);
	}

	private static bool MatchDirection(SerializerDirection searchDirection, SerializerDirection mapDirection)
	{
		if (searchDirection == SerializerDirection.Both) return true;
		if (mapDirection == SerializerDirection.Both) return true;

		return searchDirection == mapDirection;
	}
}