using System;
using System.Runtime.Serialization;

namespace FluentSerializer.Core.SerializerException;

/// <summary>
/// This exception is thrown when a class has no profile or the correct assembly containing this profile has not been registered
/// </summary>
[Serializable]
public sealed class ClassMapNotFoundException : SerializerException
{
	/// <summary>
	/// The class type attempted to lookup
	/// </summary>
	public Type TargetType { get; }

	/// <inheritdoc />
	public ClassMapNotFoundException(in Type targetType) : base(
		$"No ClassMap found for '{targetType.FullName}' \n" +
		"Make sure you've created a profile for it.")
	{
		TargetType = targetType;
	}

	#region Serializable
	private ClassMapNotFoundException(in SerializationInfo info, in StreamingContext context) : base(in info, in context)
	{
		TargetType = (Type)info.GetValue(nameof(TargetType), typeof(Type))!;
	}

	/// <inheritdoc />
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue(nameof(TargetType), TargetType);

		base.GetObjectData(info, context);
	}
	#endregion Serializable
}