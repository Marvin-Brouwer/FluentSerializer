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
		PropertyInfo property, Type classType, 
		INamingStrategy namingStrategy, ISerializer currentSerializer,
		IScanList<PropertyInfo, IPropertyMap> propertyMappings,
		IScanList<(Type type, SerializerDirection direction), IClassMap> classMappings) :
		base(classMappings)
	{
		_propertyMappings = propertyMappings;

		Property = property;
		PropertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
		ClassType = Nullable.GetUnderlyingType(classType) ?? classType;
		NamingStrategy = namingStrategy;
		CurrentSerializer = currentSerializer;
	}

	/// <inheritdoc />
	public INamingStrategy? FindNamingStrategy(PropertyInfo property) => FindNamingStrategy(_propertyMappings, property);
}