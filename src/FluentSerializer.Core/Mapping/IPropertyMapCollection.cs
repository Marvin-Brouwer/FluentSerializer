using FluentSerializer.Core.Configuration;

using System.Collections.Generic;
using System.Reflection;

namespace FluentSerializer.Core.Mapping;

/// <summary>
/// A collection of <see cref="IPropertyMap"/> with dedicated selection methods
/// </summary>
public interface IPropertyMapCollection
{
	/// <summary>
	/// Return the full collection of <see cref="IPropertyMap"/>
	/// </summary>
	IReadOnlyCollection<IPropertyMap> GetAllPropertyMaps(in SerializerDirection direction);

	/// <summary>
	/// Get a specific <see cref="IPropertyMap"/> matching <paramref name="propertyInfo"/> and <paramref name="direction"/>
	/// </summary>
	IPropertyMap? GetPropertyMapFor(in PropertyInfo propertyInfo, in SerializerDirection direction);

	/// <summary>
	/// Get a specific <see cref="IPropertyMap"/> matching only the <paramref name="propertyInfo"/>
	/// </summary>
	IPropertyMap? GetPropertyMapFor(in PropertyInfo propertyInfo);
}