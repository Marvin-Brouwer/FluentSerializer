using System.Collections.Generic;
using System.Reflection;
using FluentSerializer.Core.Configuration;

namespace FluentSerializer.Core.Mapping;

public interface IPropertyMapCollection
{
	IReadOnlyList<IPropertyMap> GetAllPropertyMaps();
	IPropertyMap? GetPropertyMapFor(in PropertyInfo propertyInfo, in SerializerDirection direction);
}