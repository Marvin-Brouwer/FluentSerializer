using System;
using System.ComponentModel;

namespace FluentSerializer.Core.Converting.Converters;

/// <summary>
/// Formatting configuration for <see cref="EnumConverterBase"/>
/// </summary>
[Flags]
public enum EnumFormat
{
	/// <summary>
	/// Use the <see cref="DescriptionAttribute" /> to read and write, read and write to name if not found, read from underlying number as fallback
	/// </summary>
	[Description("Use the most flexible format, using all EnumFormats combined")]
	Default = UseDescription | UseName | UseNumberValue,

	/// <summary>
	/// Use the <see cref="DescriptionAttribute" /> to read and write
	/// </summary>
	[Description("Use the System.ComponentModel.DescriptionAttribute to write out")]
	UseDescription = 0x1,
	/// <summary>
	/// Use the member name to read and write
	/// </summary>
	[Description("Use the name to write out")]
	UseName = 0x2,
	/// <summary>
	/// Use the underlying number type to read and write
	/// </summary>
	[Description("Use the number value to write out")]
	UseNumberValue = 0x4
}