using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Services;
using System;
using System.Reflection;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Naming.NamingStrategies;
using FluentSerializer.Core.DataNodes;

namespace FluentSerializer.Core.Mapping;

/// <summary>
/// A set of poperties describing how a Property should be mapped to a serializable representation
/// </summary>
public interface IPropertyMap
{
	/// <summary>
	/// An optional <see cref="IConverter"/> attributed to this property value
	/// </summary>
	IConverter? CustomConverter { get; }
	/// <summary>
	/// The <see cref="SerializerDirection"/> this mapping is allowed to be used in
	/// </summary>
	SerializerDirection Direction { get; }
	/// <summary>
	/// The <see cref="INamingStrategy"/> attributed to this property value
	/// </summary>
	INamingStrategy NamingStrategy { get; }
	/// <summary>
	/// The property on the class being serialized
	/// </summary>
	PropertyInfo Property { get; }
	/// <summary>
	/// The actual top level instance type of this property value
	/// </summary>
	Type ConcretePropertyType { get; }
	/// <summary>
	/// The actual top level instance type of this properties containing class
	/// </summary>
	Type ContainerType { get; }

	/// <summary>
	/// Get the best matching <see cref="IConverter"/> for this properties value
	/// </summary>
	/// <remarks>
	/// If no match is found in both this propertymap and the default converters,
	/// a <see cref="SerializerException.ConverterNotSupportedException"/> should be thrown
	/// </remarks>
	IConverter<TDataContainer, TDataNode>? GetConverter<TDataContainer, TDataNode>(
		SerializerDirection direction, in ISerializer currentSerializer)
		where TDataContainer : IDataNode
		where TDataNode : IDataNode;
}