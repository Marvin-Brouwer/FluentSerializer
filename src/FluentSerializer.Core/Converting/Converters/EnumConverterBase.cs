using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using FluentSerializer.Core.Configuration;

namespace FluentSerializer.Core.Converting.Converters;

/// <summary>
/// Converts <see cref="Enum"/>
/// </summary>
public abstract class EnumConverterBase
{
	private static InvalidCastException UnknownEnumFormatException(in string value) => new InvalidCastException(
		$"The value provided '{value}' was not present in the enum");

	private static NotSupportedException DescriptionNotFoundException(in Type enumType, in string member) =>
		new NotSupportedException(
			$"The value of '{member}' on '{enumType.FullName}' does not have a description");
	private static NotSupportedException ValueNotFoundException(in Type enumType, in string member) =>
		new NotSupportedException(
			$"The value '{member}' was not found on enum '{enumType.FullName}'");

	/// <summary>
	/// Currently configured <inheritdoc cref="EnumFormat"/>
	/// </summary>
	protected readonly EnumFormat EnumFormat;

	/// <inheritdoc cref="IConverter.Direction" />
	public virtual SerializerDirection Direction { get; } = SerializerDirection.Both;

	/// <inheritdoc cref="IConverter.CanConvert(in Type)" />
	public virtual bool CanConvert(in Type targetType) => targetType.IsEnum;

	/// <inheritdoc cref="EnumConverterBase"/>
	/// <paramref name="enumFormat">The format to use when reading and writing serialized <c>enum</c> values</paramref>
	protected EnumConverterBase(EnumFormat enumFormat)
	{
		EnumFormat = enumFormat;
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
			EnumFormat.UseDescription | EnumFormat.UseName =>
				GetEnumDescription(in memberName, in enumType) ??
				enumMemberNameValue,
			EnumFormat.UseDescription | EnumFormat.UseNumberValue =>
				GetEnumDescription(in memberName, in enumType) ??
				GetEnumUnderlyingValue(value),
			EnumFormat.UseDescription | EnumFormat.UseName | EnumFormat.UseNumberValue =>
				GetEnumDescription(in memberName, in enumType) ??
				enumMemberNameValue,
			EnumFormat.UseName | EnumFormat.UseNumberValue =>
				enumMemberNameValue,
			EnumFormat.UseDescription =>
				GetEnumDescription(in memberName, in enumType) ??
				throw DescriptionNotFoundException(value.GetType(), in memberName),
			EnumFormat.UseName =>
				enumMemberNameValue,
			EnumFormat.UseNumberValue =>
				GetEnumUnderlyingValue(value),

			// This should really never happen:
			_ => throw UnknownEnumFormatException(EnumFormat.ToString())
		};
	}

	private static (string value, bool isNumeric) GetEnumUnderlyingValue(object value)
	{
		var underlyingType = Enum.GetUnderlyingType(value.GetType());
		var numberValue = Convert.ChangeType(value, underlyingType);

		return (Convert.ToString(numberValue)!, true);
	}

	private static string GetEnumNameValue(in object value)
	{
		return value.ToString()!;
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
			EnumFormat.UseDescription | EnumFormat.UseName =>
				GetEnumFromDescription(in currentValue, in targetType) ??
				GetEnumFromName(in currentValue, in targetType),
			EnumFormat.UseDescription | EnumFormat.UseNumberValue =>
				GetEnumFromDescription(in currentValue, in targetType) ??
				GetEnumFromNumber(in currentValue, in targetType),
			EnumFormat.UseName | EnumFormat.UseNumberValue =>
				GetEnumFromName(in currentValue, in targetType) ??
				GetEnumFromNumber(in currentValue, in targetType),
			EnumFormat.UseDescription | EnumFormat.UseName | EnumFormat.UseNumberValue =>
				GetEnumFromDescription(in currentValue, in targetType) ??
				GetEnumFromName(in currentValue, in targetType) ??
				GetEnumFromNumber(in currentValue, in targetType),

			EnumFormat.UseDescription => GetEnumFromDescription(in currentValue, in targetType),
			EnumFormat.UseName => GetEnumFromName(in currentValue, in targetType),
			EnumFormat.UseNumberValue => GetEnumFromNumber(in currentValue, in targetType),

			// This should really never happen:
			_ => throw UnknownEnumFormatException(EnumFormat.ToString())
		}; 
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

				var description = ((DescriptionAttribute)valueAttributes[0])?.Description;
				if (description is null) continue;

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

	private static object? GetEnumFromNumber(in string? currentValue, in Type targetType)
	{
		try
		{
			var underlyingType = Enum.GetUnderlyingType(targetType);
			var numberValue = Convert.ChangeType(currentValue, underlyingType);
			if (numberValue is null) return default;

			return Enum.ToObject(targetType, numberValue);
		}
		catch
		{
			return default;
		}
	}

	/// <inheritdoc />
	public override int GetHashCode() => typeof(System.Enum).GetHashCode();
}
