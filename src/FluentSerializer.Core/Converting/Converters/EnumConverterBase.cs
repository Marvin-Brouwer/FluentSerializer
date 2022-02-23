using System;
using System.ComponentModel;
using System.Reflection;
using FluentSerializer.Core.Configuration;

namespace FluentSerializer.Core.Converting.Converters;

/// <summary>
/// Converts <see cref="Enum"/>
/// </summary>
public abstract class EnumConverterBase
{
	private static InvalidCastException UnknownEnumFormatException (in string value) => new InvalidCastException(
		$"The value provided '{value}' was not present in the enum");
	private static NotSupportedException DescriptionNotFoundException (in Type enumType, in string member) => new NotSupportedException(
		$"The value of '{member}' on '{enumType.FullName}' does not have a description");

	private readonly EnumOutputFormat _enumOutputFormat;

	/// <inheritdoc cref="IConverter.Direction" />
	public virtual SerializerDirection Direction { get; } = SerializerDirection.Both;

	/// <inheritdoc cref="IConverter.CanConvert(in Type)" />
	public virtual bool CanConvert(in Type targetType) => targetType.IsEnum;

	/// <inheritdoc cref="EnumConverterBase"/>
	protected EnumConverterBase(EnumOutputFormat enumOutputFormat)
	{
		_enumOutputFormat = enumOutputFormat;
	}

	/// <summary>
	/// Wrapper around <see cref="Convert.ToString(bool)"/>
	/// </summary>
	protected virtual string? ConvertToString(in object value)
	{
		var name = GetEnumNameValue(value);
		switch (_enumOutputFormat)
		{
			case EnumOutputFormat.UseDescription | EnumOutputFormat.UseName:
				return GetEnumDescription(in name) ?? name;
			case EnumOutputFormat.UseDescription | EnumOutputFormat.UseNumberValue:
				return GetEnumDescription(in name) ?? GetEnumUnderlyingValue(value);
			case EnumOutputFormat.UseName | EnumOutputFormat.UseNumberValue:
				return name;

			case EnumOutputFormat.UseDescription:
				return GetEnumDescription(in name) ?? throw DescriptionNotFoundException(value.GetType(), in name);
			case EnumOutputFormat.UseName:
				return name;
			case EnumOutputFormat.UseNumberValue:
				return GetEnumUnderlyingValue(value);
		}

		// This should really never happen
		throw UnknownEnumFormatException(_enumOutputFormat.ToString());
	}

	private static string? GetEnumUnderlyingValue(object value)
	{
		var underlyingType = Enum.GetUnderlyingType(value.GetType());
		var numberValue = Convert.ChangeType(value, underlyingType);

		return Convert.ToString(numberValue);
	}

	private static string GetEnumNameValue(in object value)
	{
		return value.ToString()!;
	}

	private static string? GetEnumDescription(in string name)
	{
		try
		{
			var memberInfo = GetEnumMemberInfo(in name);
			if (memberInfo is null) return null;

			var valueAttributes = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
			return ((DescriptionAttribute)valueAttributes[0]).Description;
		}
		catch
		{
			return null;
		}
	}

	private static MemberInfo? GetEnumMemberInfo(in string name)
	{
		var enumType = typeof(EnumOutputFormat);
		var memberInfos = enumType.GetMember(name);
		foreach(var memberInfo in memberInfos)
		{
			if (memberInfo.DeclaringType == enumType) return memberInfo;
		}

		return null;
	}

	/// <summary>
	/// Wrapper around <see cref="Convert.ChangeType(object?, Type)"/> to support nullable values
	/// </summary>
	protected virtual object? ConvertToNullableDataType(in string? currentValue, in Type targetType)
	{
		if (string.IsNullOrWhiteSpace(currentValue)) return default;

		return Convert.ChangeType(currentValue, targetType);
	}
}
