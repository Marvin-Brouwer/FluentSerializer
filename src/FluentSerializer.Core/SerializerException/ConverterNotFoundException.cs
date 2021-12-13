using FluentSerializer.Core.Configuration;
using System;
using System.Runtime.Serialization;

namespace FluentSerializer.Core.SerializerException;

[Serializable]
public sealed class ConverterNotFoundException : SerializerException
{
	public Type TargetType { get; }
	public Type ContainerType { get; }
	public SerializerDirection Direction { get; }

	public ConverterNotFoundException(Type targetType, Type containerType, SerializerDirection direction) : base(
		$"No IConverter found for '{targetType.FullName}' \n" +
		"Make sure you've registered or selected a converter that supports this conversion.")
	{
		TargetType = targetType;
		ContainerType = containerType;
		Direction = direction;
	}

	#region Serializable
	private ConverterNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
		TargetType = (Type)info.GetValue(nameof(TargetType), typeof(Type))!;
		ContainerType = (Type)info.GetValue(nameof(ContainerType), typeof(Type))!;
		Direction = (SerializerDirection)info.GetValue(nameof(Direction), typeof(SerializerDirection))!;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(TargetType), TargetType);
		info.AddValue(nameof(ContainerType), ContainerType);
		info.AddValue(nameof(Direction), Direction);

		base.GetObjectData(info, context);
	}
	#endregion
}