using FluentSerializer.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentSerializer.Core.Mapping;

/// <inheritdoc />
public sealed class PropertyMapScanList : ScanList<PropertyInfo, IPropertyMap>
{
	/// <inheritdoc />
	public PropertyMapScanList(in IReadOnlyCollection<IPropertyMap> dataTypes) : base(in dataTypes) { }

	/// <remarks>
	/// Because <see cref="PropertyInfo"/> isn't comparable just check important properties.
	/// </remarks>
	protected override bool Compare(PropertyInfo type, in IPropertyMap dataType)
	{
		if (!type.Name.Equals(dataType.Property.Name, StringComparison.Ordinal)) return false;
		if (type.PropertyType.EqualsTopLevel(dataType.Property.PropertyType)) return true;
		if (type.PropertyType.Implements(dataType.Property.PropertyType)) return true;

		return false;
	}
}