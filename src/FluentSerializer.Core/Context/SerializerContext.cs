using FluentSerializer.Core.Mapping;
using FluentSerializer.Core.Services;
using System;
using System.Reflection;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.Core.Context;

/// <inheritdoc cref="ISerializerContext"/>
public sealed class SerializerContext : NamingContext, ISerializerContext
{
	private readonly IScanList<PropertyInfo, IPropertyMap> _propertyMappings;

	/// <inheritdoc />
	public PropertyInfo Property { get; }
	/// <inheritdoc />
	public Type PropertyType { get; }
	/// <inheritdoc />
	public Type ClassType { get; }
	/// <inheritdoc />
	public INamingStrategy NamingStrategy  { get; }
	/// <inheritdoc />
	public ISerializer CurrentSerializer  { get; }

	/// <inheritdoc />
	public SerializerContext(
		in PropertyInfo property, in Type classType, 
		in INamingStrategy namingStrategy, in ISerializer currentSerializer,
		in IScanList<PropertyInfo, IPropertyMap> propertyMappings,
		in IScanList<(Type type, SerializerDirection direction), IClassMap> classMappings) :
		base(in classMappings)
	{
		_propertyMappings = propertyMappings;

		Property = property;
		PropertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
		ClassType = Nullable.GetUnderlyingType(classType) ?? classType;
		NamingStrategy = namingStrategy;
		CurrentSerializer = currentSerializer;
	}

	/// <inheritdoc />
	public INamingStrategy? FindNamingStrategy(in PropertyInfo property) => FindNamingStrategy(in _propertyMappings, property);
}