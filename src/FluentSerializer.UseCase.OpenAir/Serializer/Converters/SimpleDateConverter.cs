using FluentSerializer.Xml.Converting.Converters;

using System.Globalization;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Converters;

/// <summary>
/// Converter for simple date structures<br />
/// <example>
/// yyyy-MM-dd
/// </example>
/// </summary>
public sealed class SimpleDateConverter : DateTimeByFormatConverter
{
	private const string DateFormat = "yyyy-MM-dd";
	private static readonly CultureInfo CultureInfo = CultureInfo.InvariantCulture;
	private static readonly DateTimeStyles DateTimeStyle = DateTimeStyles.AssumeUniversal;

	public SimpleDateConverter() : base(DateFormat, CultureInfo, DateTimeStyle) { }
}