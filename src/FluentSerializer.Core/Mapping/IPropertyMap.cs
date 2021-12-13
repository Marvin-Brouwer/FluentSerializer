using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Services;
using System;
using System.Reflection;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Core.DataNodes;

namespace FluentSerializer.Core.Mapping;

public interface IPropertyMap
{
	IConverter? CustomConverter { get; }
	SerializerDirection Direction { get; }
	INamingStrategy NamingStrategy { get; }
	PropertyInfo Property { get; }
	Type ConcretePropertyType { get; }
	Type ContainerType { get; }

	IConverter<TDataContainer>? GetConverter<TDataContainer>(
		SerializerDirection direction, ISerializer currentSerializer)
		where TDataContainer : IDataNode;
}