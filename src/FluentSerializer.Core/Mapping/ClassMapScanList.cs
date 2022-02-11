using FluentSerializer.Core.Extensions;
using System;
using System.Collections.Generic;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Profiles;

namespace FluentSerializer.Core.Mapping;

/// <inheritdoc cref="IClassMapScanList{TSerializerProfile}" />
public sealed class ClassMapScanList<TSerializerProfile> :
	ScanList<(Type type, SerializerDirection direction), IClassMap>, IClassMapScanList<TSerializerProfile>
	where TSerializerProfile : ISerializerProfile
{
	/// <inheritdoc />
	public ClassMapScanList(in IReadOnlyList<IClassMap> dataTypes) : base(in dataTypes) { }

	/// <inheritdoc />
	protected override bool Compare((Type type, SerializerDirection direction) compareTo, in IClassMap dataType)
	{
		if (!MatchDirection(compareTo.direction, dataType.Direction)) return false;

		var concreteCompareTo = Nullable.GetUnderlyingType(compareTo.type) ?? compareTo.type;
		return concreteCompareTo.EqualsTopLevel(dataType.ClassType);
	}

	private static bool MatchDirection(in SerializerDirection searchDirection, in SerializerDirection mapDirection)
	{
		if (searchDirection == SerializerDirection.Both) return true;
		if (mapDirection == SerializerDirection.Both) return true;

		return searchDirection == mapDirection;
	}

	/// <summary>
	/// Join the data of two <see cref="ClassMapScanList{TSerializerProfile}"/>s
	/// </summary>
	public ClassMapScanList<TSerializerProfile> Append(in IClassMapScanList<TSerializerProfile> classMaps)
	{
		var dataTypes = new List<IClassMap>();
		dataTypes.AddRange(this);
		dataTypes.AddRange(classMaps);

		return new ClassMapScanList<TSerializerProfile>(dataTypes);
	}
}