using FluentSerializer.Core.Extensions;
using System;
using System.Collections.Generic;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Profiles;

namespace FluentSerializer.Core.Mapping;

/// <inheritdoc cref="IClassMapScanList{TSerializerProfile, TConfiguration}" />
public sealed class ClassMapScanList<TSerializerProfile, TConfiguration> :
	ScanList<(Type type, SerializerDirection direction), IClassMap>, IClassMapScanList<TSerializerProfile, TConfiguration>
	where TSerializerProfile : ISerializerProfile<TConfiguration>
	where TConfiguration : ISerializerConfiguration
{
	/// <inheritdoc />
	public ClassMapScanList(in IReadOnlyCollection<IClassMap> dataTypes) : base(in dataTypes) { }

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
	/// Join the data of two <see cref="ClassMapScanList{TSerializerProfile, TConfiguration}"/>s
	/// </summary>
	public ClassMapScanList<TSerializerProfile, TConfiguration> Append(in IClassMapScanList<TSerializerProfile, TConfiguration> classMaps)
	{
		var dataTypes = new List<IClassMap>();
		dataTypes.AddRange(this);
		dataTypes.AddRange(classMaps);

		return new ClassMapScanList<TSerializerProfile, TConfiguration>(dataTypes);
	}
}