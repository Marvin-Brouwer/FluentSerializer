using System;
using System.Runtime.Serialization;

namespace FluentSerializer.Core.SerializerException;

[Serializable]
public sealed class ClassMapNotFoundException : SerializerException
{
	public Type TargetType { get; }

	public ClassMapNotFoundException(Type targetType) : base(
		$"No ClassMap found for '{targetType.FullName}' \n" +
		"Make sure you've created a profile for it.")
	{
		TargetType = targetType;
	}

	#region Serializable
	private ClassMapNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
		TargetType = (Type)info.GetValue(nameof(TargetType), typeof(Type))!;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(TargetType), TargetType);

		base.GetObjectData(info, context);
	}
	#endregion
}