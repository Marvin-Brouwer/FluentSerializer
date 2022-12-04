using FluentSerializer.Core.Configuration;

using System;

namespace FluentSerializer.Core.Converting.Converters;

/// <summary>
/// This class contains methods that are used when converting simple types
/// </summary>
public abstract class SimpleTypeConverterBase<TObject> : IConverter
{
	/// <inheritdoc cref="IConverter.Direction" />
	public virtual SerializerDirection Direction { get; } = SerializerDirection.Both;
	/// <inheritdoc cref="IConverter.CanConvert(in Type)" />
	public virtual bool CanConvert(in Type targetType) => typeof(TObject).IsAssignableFrom(targetType);

	/// <inheritdoc />
	public Guid ConverterId { get; } = typeof(TObject).GUID;

	/// <summary>
	/// Abstract placeholder for converting to string logic
	/// </summary>
	protected abstract string ConvertToString(in TObject value);
	/// <summary>
	/// Abstract placeholder for converting to object logic
	/// </summary>
	protected abstract TObject ConvertToDataType(in string currentValue);

	/// <summary>
	/// Wrapper around <see cref="ConvertToDataType(in string)"/> to support nullable values
	/// </summary>
	protected virtual TObject? ConvertToNullableDataType(in string? currentValue)
	{
		if (currentValue is null) return default;

		return ConvertToDataType(in currentValue);
	}

	/// <inheritdoc cref="object.GetHashCode" />
	public override int GetHashCode() => typeof(TObject).GetHashCode();

	/// <inheritdoc cref="object.Equals(object?)" />
	public override bool Equals(object? obj) => GetHashCode() == (obj?.GetHashCode() ?? 0);
}