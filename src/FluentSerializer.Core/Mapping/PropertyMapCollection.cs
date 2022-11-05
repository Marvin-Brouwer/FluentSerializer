using Ardalis.GuardClauses;

using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentSerializer.Core.Mapping;

/// <inheritdoc />
public readonly struct PropertyMapCollection : IPropertyMapCollection
{
	private readonly IReadOnlyCollection<IPropertyMap> _propertyMaps;

	/// <inheritdoc cref="IPropertyMapCollection" />
	public PropertyMapCollection(in IReadOnlyCollection<IPropertyMap> propertyMaps)
	{
		_propertyMaps = propertyMaps;
	}

	/// <inheritdoc />
	public IReadOnlyCollection<IPropertyMap> GetAllPropertyMaps(in SerializerDirection direction)
	{
		Guard.Against.InvalidChoice(
			direction, SerializerDirection.Both,
			$"You cannot get a {nameof(PropertyMap)} for {nameof(SerializerDirection)}.{SerializerDirection.Both} \n" +
			"you can only register one as such!"
		);

		return GetPropertyMaps(direction).ToArray();
	}

	private IEnumerable<IPropertyMap> GetPropertyMaps(SerializerDirection direction)
	{
		foreach (var propertyMap in _propertyMaps)
		{
			if (!MatchDirection(in direction, propertyMap.Direction)) continue;

			yield return propertyMap;
		}
	}

	/// <inheritdoc />
	public IPropertyMap? GetPropertyMapFor(in PropertyInfo propertyInfo, in SerializerDirection direction)
	{
		Guard.Against.InvalidChoice(
			direction, SerializerDirection.Both,
			$"You cannot get a {nameof(PropertyMap)} for {nameof(SerializerDirection)}.{SerializerDirection.Both} \n" +
			"you can only register one as such!"
		);

		foreach (var propertyMap in _propertyMaps)
		{
			if (!MatchDirection(in direction, propertyMap.Direction)) continue;
			if (!MatchProperty(propertyMap.Property, in propertyInfo)) continue;

			return propertyMap;
		}

		return default;
	}

	/// <inheritdoc />
	public IPropertyMap? GetPropertyMapFor(in PropertyInfo propertyInfo)
	{
		foreach (var propertyMap in _propertyMaps)
		{
			if (!MatchProperty(propertyMap.Property, in propertyInfo)) continue;

			return propertyMap;
		}

		return default;
	}

	private static bool MatchDirection(in SerializerDirection searchDirection, in SerializerDirection mapDirection)
	{
		if (searchDirection == SerializerDirection.Both) return true;
		if (mapDirection == SerializerDirection.Both) return true;

		return searchDirection == mapDirection;
	}

	private static bool MatchProperty(in PropertyInfo propertyMapPropertyInfo, in PropertyInfo propertyInfo)
	{
		if (!propertyInfo.Name.Equals(propertyMapPropertyInfo.Name, StringComparison.Ordinal)) return false;
		if (propertyInfo.PropertyType.EqualsTopLevel(propertyMapPropertyInfo.PropertyType)) return true;
		if (propertyInfo.PropertyType.Implements(propertyMapPropertyInfo.PropertyType)) return true;

		return false;
	}
}
