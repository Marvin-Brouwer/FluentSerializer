using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;

using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace FluentSerializer.Core.SerializerException;

/// <summary>
/// This exception will be thrown if you attribute a serializer that doesn't support the property's or direction type.
/// </summary>
[Serializable]
public sealed class ConverterNotSupportedException : SerializerException
{
	/// <summary>
	/// The type attempted to serialize
	/// </summary>
	public Type TargetType { get; }
	/// <summary>
	/// The property attempted to convert
	/// </summary>
	public PropertyInfo Property { get; }
	/// <summary>
	/// The type of the attributed converter
	/// </summary>
	public Type ConverterType { get; }
	/// <summary>
	/// The node type (eg. attribute, element etc.)
	/// </summary>
	public Type ContainerType { get; }

	/// <summary>
	/// The direction attempted to convert
	/// </summary>
	public SerializerDirection Direction { get; }

	/// <inheritdoc />
	public ConverterNotSupportedException(in IPropertyMap property, in Type converterType, in Type containerType, in SerializerDirection direction) : base(
		$"The converter of type '{converterType}' selected for '{property.ContainerType.FullName ?? "<dynamic>"}.{property.Property.Name}' cannot convert '{property.ConcretePropertyType.FullName}' \n" +
		"Make sure you've selected a converter that supports this conversion.")
	{
		TargetType = property.ConcretePropertyType;
		Property = property.Property;
		ContainerType = containerType;
		ConverterType = converterType;
		Direction = direction;
	}

	#region Serializable
	private ConverterNotSupportedException(in SerializationInfo info, in StreamingContext context) : base(in info, in context)
	{
		TargetType = (Type)info.GetValue(nameof(TargetType), typeof(Type))!;
		Property = (PropertyInfo)info.GetValue(nameof(Property), typeof(PropertyInfo))!;
		ConverterType = (Type)info.GetValue(nameof(ConverterType), typeof(Type))!;
		ContainerType = (Type)info.GetValue(nameof(ContainerType), typeof(Type))!;
		Direction = (SerializerDirection)info.GetValue(nameof(Direction), typeof(SerializerDirection))!;
	}

	/// <inheritdoc />
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(TargetType), TargetType);
		info.AddValue(nameof(Property), Property);
		info.AddValue(nameof(ConverterType), ConverterType);
		info.AddValue(nameof(ContainerType), ContainerType);
		info.AddValue(nameof(Direction), Direction);

		base.GetObjectData(info, context);
	}
	#endregion
}