using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Extensions;

namespace FluentSerializer.Core.Mapping;

public readonly struct PropertyMapCollection : IPropertyMapCollection
{
	private readonly IReadOnlyList<IPropertyMap> _propertyMaps;

	public PropertyMapCollection(IReadOnlyList<IPropertyMap> propertyMaps)
	{
		_propertyMaps = propertyMaps;
	}

	public IReadOnlyList<IPropertyMap> GetAllPropertyMaps() => _propertyMaps;

	public IPropertyMap? GetPropertyMapFor(in PropertyInfo propertyInfo, in SerializerDirection direction)
	{
		// todo cache?
		if (direction == SerializerDirection.Both)
			throw new NotSupportedException(
				$"You cannot get a {nameof(PropertyMap)} for {nameof(SerializerDirection)}.{SerializerDirection.Both} \n" +
				"you can only register one as such!");

		foreach (var propertyMap in _propertyMaps)
		{
			if (!MatchDirection(in direction, propertyMap.Direction)) continue;
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
