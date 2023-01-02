using FluentSerializer.Core.Configuration;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace FluentSerializer.Core.Converting.Converters;

/// <summary>
/// Converts <see cref="Enum"/>
/// </summary>
public abstract class EnumConverterBase
{
	private static InvalidCastException UnknownEnumFormatException(in string value) => new(
		$"The value provided '{value}' was not present in the enum");

	private static NotSupportedException ValueNotFoundException(in Type enumType, in string member) => new(
		$"The value '{member}' was not found on enum '{enumType.FullName}'");

	/// <summary>
	/// Currently configured <inheritdoc cref="EnumFormat"/>
	/// </summary>
	protected virtual EnumFormats EnumFormat { get; }

	/// <inheritdoc cref="IConverter.Direction" />
	public virtual SerializerDirection Direction { get; } = SerializerDirection.Both;

	/// <inheritdoc cref="IConverter.CanConvert(in Type)" />
	public virtual bool CanConvert(in Type targetType) => targetType.IsEnum;

	/// <inheritdoc cref="IConverter.ConverterId" />
	public Guid ConverterId { get; } = typeof(Enum).GUID;

	private readonly CultureInfo? _formatProvider;
	private CultureInfo FormatProvider => _formatProvider ?? CultureInfo.CurrentCulture;

	/// <inheritdoc cref="EnumConverterBase"/>
	/// <paramref name="enumFormat">The format to use when reading and writing serialized <c>enum</c> values</paramref>
	protected EnumConverterBase(EnumFormats enumFormat, CultureInfo? formatProvider)
	{
		EnumFormat = enumFormat;
		_formatProvider = formatProvider;
	}

	/// <summary>
	/// Wrapper around <see cref="Convert.ToString(bool)"/>
	/// </summary>
	protected virtual (string value, bool isNumeric)? ConvertToString(in object value, in Type enumType)
	{
		var memberName = GetEnumNameValue(value);
		var enumMemberNameValue = (nameValue: memberName, false);
		return EnumFormat switch
		{
			EnumFormats.UseEnumMember | EnumFormats.UseDescription | EnumFormats.UseName =>
				GetEnumMemberValue(in memberName, in enumType) ??
				GetEnumDescription(in memberName, in enumType) ??
				enumMemberNameValue,
			EnumFormats.UseEnumMember | EnumFormats.UseDescription | EnumFormats.UseNumberValue =>
				GetEnumMemberValue(in memberName, in enumType) ??
				GetEnumDescription(in memberName, in enumType) ??
				GetEnumUnderlyingValue(value),
			EnumFormats.UseEnumMember | EnumFormats.UseDescription | EnumFormats.UseName | EnumFormats.UseNumberValue =>
				GetEnumMemberValue(in memberName, in enumType) ??
				GetEnumDescription(in memberName, in enumType) ??
				enumMemberNameValue,

			EnumFormats.UseEnumMember | EnumFormats.UseName =>
				GetEnumMemberValue(in memberName, in enumType) ??
				enumMemberNameValue,
			EnumFormats.UseEnumMember | EnumFormats.UseNumberValue =>
				GetEnumMemberValue(in memberName, in enumType) ??
				GetEnumUnderlyingValue(value),
			EnumFormats.UseEnumMember | EnumFormats.UseName | EnumFormats.UseNumberValue =>
				GetEnumMemberValue(in memberName, in enumType) ??
				enumMemberNameValue,

			EnumFormats.UseDescription | EnumFormats.UseName =>
				GetEnumDescription(in memberName, in enumType) ??
				enumMemberNameValue,
			EnumFormats.UseDescription | EnumFormats.UseNumberValue =>
				GetEnumDescription(in memberName, in enumType) ??
				GetEnumUnderlyingValue(value),
			EnumFormats.UseDescription | EnumFormats.UseName | EnumFormats.UseNumberValue =>
				GetEnumDescription(in memberName, in enumType) ??
				enumMemberNameValue,

			EnumFormats.UseName | EnumFormats.UseNumberValue =>
				enumMemberNameValue,

			EnumFormats.UseEnumMember =>
				GetEnumMemberValue(in memberName, in enumType) ??
				throw ValueNotFoundException(value.GetType(), in memberName),
			EnumFormats.UseDescription =>
				GetEnumDescription(in memberName, in enumType) ??
				throw ValueNotFoundException(value.GetType(), in memberName),
			EnumFormats.UseName =>
				enumMemberNameValue,
			EnumFormats.UseNumberValue =>
				GetEnumUnderlyingValue(value),

			// This should really never happen:
			_ => throw UnknownEnumFormatException(EnumFormat.ToString())
		};
	}

	private (string value, bool isNumeric) GetEnumUnderlyingValue(object value)
	{
		var underlyingType = Enum.GetUnderlyingType(value.GetType());
		var numberValue = Convert.ChangeType(value, underlyingType, FormatProvider);

		return (Convert.ToString(numberValue, FormatProvider)!, true);
	}

	private static string GetEnumNameValue(in object value)
	{
		return value.ToString()!;
	}

	private static (string value, bool isNumeric)? GetEnumMemberValue(in string name, in Type enumType)
	{
		try
		{
			var memberInfo = GetEnumMemberInfo(in name, in enumType);
			if (memberInfo is null) return null;

			var valueAttributes = memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false);
			var enumMemberValue = ((EnumMemberAttribute)valueAttributes[0]).Value;
			if (enumMemberValue is null) return null;

			return (enumMemberValue, false);
		}
		catch
		{
			return null;
		}
	}

	private static (string value, bool isNumeric)? GetEnumDescription(in string name, in Type enumType)
	{
		try
		{
			var memberInfo = GetEnumMemberInfo(in name, in enumType);
			if (memberInfo is null) return null;

			var valueAttributes = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
			var description = ((DescriptionAttribute)valueAttributes[0]).Description;
			return (description, false);
		}
		catch
		{
			return null;
		}
	}

	private static MemberInfo? GetEnumMemberInfo(in string name, in Type enumType)
	{
		var memberInfos = enumType.GetMember(name);
		foreach (var memberInfo in memberInfos)
		{
			if (memberInfo.DeclaringType == enumType) return memberInfo;
		}

		return null;
	}

	private static IEnumerable<MemberInfo> GetEnumMemberInfos(Type enumType)
	{
		var memberInfos = enumType.GetMembers();
		foreach (var memberInfo in memberInfos)
		{
			if (memberInfo.DeclaringType == enumType) yield return memberInfo;
		}
	}

	/// <summary>
	/// Wrapper around <see cref="Enum.TryParse(Type,string,bool,out object)"/>
	/// </summary>
	protected virtual object? ConvertToEnum(in string? currentValue, in Type targetType)
	{
		if (currentValue is null) return default;

		return EnumFormat switch
		{
			EnumFormats.UseEnumMember | EnumFormats.UseDescription | EnumFormats.UseName =>
				GetEnumFromEnumMember(in currentValue, in targetType) ??
				GetEnumFromDescription(in currentValue, in targetType) ??
				GetEnumFromName(in currentValue, in targetType),
			EnumFormats.UseEnumMember | EnumFormats.UseDescription | EnumFormats.UseNumberValue =>
				GetEnumFromEnumMember(in currentValue, in targetType) ??
				GetEnumFromDescription(in currentValue, in targetType) ??
				GetEnumFromNumber(in currentValue, in targetType),
			EnumFormats.UseEnumMember | EnumFormats.UseDescription | EnumFormats.UseName | EnumFormats.UseNumberValue =>
				GetEnumFromEnumMember(in currentValue, in targetType) ??
				GetEnumFromDescription(in currentValue, in targetType) ??
				GetEnumFromName(in currentValue, in targetType) ??
				GetEnumFromNumber(in currentValue, in targetType),

			EnumFormats.UseEnumMember | EnumFormats.UseName =>
				GetEnumFromEnumMember(in currentValue, in targetType) ??
				GetEnumFromName(in currentValue, in targetType),
			EnumFormats.UseEnumMember | EnumFormats.UseNumberValue =>
				GetEnumFromEnumMember(in currentValue, in targetType) ??
				GetEnumFromNumber(in currentValue, in targetType),
			EnumFormats.UseEnumMember | EnumFormats.UseName | EnumFormats.UseNumberValue =>
				GetEnumFromEnumMember(in currentValue, in targetType) ??
				GetEnumFromName(in currentValue, in targetType) ??
				GetEnumFromNumber(in currentValue, in targetType),

			EnumFormats.UseDescription | EnumFormats.UseName =>
				GetEnumFromDescription(in currentValue, in targetType) ??
				GetEnumFromName(in currentValue, in targetType),
			EnumFormats.UseDescription | EnumFormats.UseNumberValue =>
				GetEnumFromDescription(in currentValue, in targetType) ??
				GetEnumFromNumber(in currentValue, in targetType),
			EnumFormats.UseDescription | EnumFormats.UseName | EnumFormats.UseNumberValue =>
				GetEnumFromDescription(in currentValue, in targetType) ??
				GetEnumFromName(in currentValue, in targetType) ??
				GetEnumFromNumber(in currentValue, in targetType),

			EnumFormats.UseName | EnumFormats.UseNumberValue =>
				GetEnumFromName(in currentValue, in targetType) ??
				GetEnumFromNumber(in currentValue, in targetType),

			EnumFormats.UseEnumMember => GetEnumFromEnumMember(in currentValue, in targetType),
			EnumFormats.UseDescription => GetEnumFromDescription(in currentValue, in targetType),
			EnumFormats.UseName => GetEnumFromName(in currentValue, in targetType),
			EnumFormats.UseNumberValue => GetEnumFromNumber(in currentValue, in targetType),

			// This should really never happen:
			_ => throw UnknownEnumFormatException(EnumFormat.ToString())
		};
	}

	private static object? GetEnumFromEnumMember(in string currentValue, in Type targetType)
	{
		try
		{
			var memberInfos = GetEnumMemberInfos(targetType);
			foreach (var memberInfo in memberInfos)
			{
				var valueAttributes = memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false);
				if (valueAttributes.Length == 0) continue;

				var description = ((EnumMemberAttribute)valueAttributes[0]).Value;
				if (string.IsNullOrWhiteSpace(description)) continue;

				if (description.Equals(currentValue, StringComparison.OrdinalIgnoreCase))
					return Enum.Parse(targetType, memberInfo.Name);
			}

			return default;
		}
		catch
		{
			return default;
		}
	}

	private static object? GetEnumFromDescription(in string currentValue, in Type targetType)
	{
		try
		{
			var memberInfos = GetEnumMemberInfos(targetType);
			foreach (var memberInfo in memberInfos)
			{
				var valueAttributes = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
				if (valueAttributes.Length == 0) continue;

				var description = ((DescriptionAttribute)valueAttributes[0]).Description;
				if (string.IsNullOrWhiteSpace(description)) continue;

				if (description.Equals(currentValue, StringComparison.OrdinalIgnoreCase))
					return Enum.Parse(targetType, memberInfo.Name);
			}

			return default;
		}
		catch
		{
			return default;
		}
	}

	/// <summary>
	/// Wrap <see cref="Enum.Parse(Type, string)"/> in a check by name to prevent number values matching.
	/// </summary>
	private static object? GetEnumFromName(in string? currentValue, in Type targetType)
	{
		var names = Enum.GetNames(targetType);

		foreach (var name in names)
		{
			if (name.Equals(currentValue, StringComparison.OrdinalIgnoreCase))
				return Enum.Parse(targetType, name);
		}

		return default;
	}

	private object? GetEnumFromNumber(in string? currentValue, in Type targetType)
	{
		try
		{
			var underlyingType = Enum.GetUnderlyingType(targetType);
			var numberValue = Convert.ChangeType(currentValue, underlyingType, FormatProvider);
			if (numberValue is null) return default;

			return Enum.ToObject(targetType, numberValue);
		}
		catch
		{
			return default;
		}
	}
}
