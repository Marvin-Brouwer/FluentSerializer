using FluentAssertions;

using FluentSerializer.Core.Converting.Converters;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Converting.Converters;

/// <summary>
/// Basically test if this converter behaves exactly like <see cref="Convert.ToString(bool)"/>
/// and <see cref="Convert.ChangeType(object?, Type)"/>
/// </summary>
public sealed class FormattibleConverterBaseTests
{
	public FormattibleConverterBaseTests()
	{
		Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
	}

	public static IEnumerable<object[]> GenerateFormattableData()
	{
		yield return new object[] { 1, null!, "1" };
		yield return new object[] { new DateOnly(1991, 11, 28), null!, "28-11-1991" };
		yield return new object[] { new DateOnly(1991, 11, 28), "O", "1991-11-28" };
		yield return new object[] { new DateOnly(1991, 11, 28), "yyyy'_'MM'_'dd", "1991_11_28" };
		yield return new object[] { new TimeOnly(13, 00, 00), null!, "13:00" };
		yield return new object[] { new TimeOnly(13, 00, 00), "HH':'mm", "13:00" };
		yield return new object[] { new TimeOnly(13, 00, 00), "hh':'mm tt", "01:00 p.m." };
		yield return new object[] { 6.9, null!, "6,9" };
	}

	public static IEnumerable<object[]> GenerateCultureData()
	{
		yield return new object[] { new DateOnly(1991, 11, 28), null!, "28-11-1991" };
		yield return new object[] { new TimeOnly(12, 00, 00), null!, "12:00" };
		yield return new object[] { 6.9, null!, "6,9" };
		yield return new object[] { new DateOnly(1991, 11, 28), new CultureInfo("nl-NL"), "28-11-1991" };
		yield return new object[] { new TimeOnly(12, 00, 00), new CultureInfo("nl-NL"), "12:00" };
		yield return new object[] { 6.9, new CultureInfo("nl-NL"), "6,9" };
		yield return new object[] { new DateOnly(1991, 11, 28), new CultureInfo("en-GB"), "28/11/1991" };
		yield return new object[] { new TimeOnly(12, 00, 00), new CultureInfo("en-GB"), "12:00" };
		yield return new object[] { 6.9, new CultureInfo("en-GB"), "6.9" };
		yield return new object[] { new DateOnly(1991, 11, 28), new CultureInfo("de-DE"), "28.11.1991" };
		yield return new object[] { new TimeOnly(12, 00, 00), new CultureInfo("de-DE"), "12:00" };
		yield return new object[] { 6.9, new CultureInfo("de-DE"), "6,9" };
	}

	#region ConvertToString

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void ConvertToString_NonFormattable_Throws()
	{
		// Arrange
		using var input = new MemoryStream(0);
		var sut = new TestConverter(null, null);

		// Act
		var canConvert = sut.CanConvert(input.GetType());
		var result = () => sut.ConvertToString(input);

		// Assert
		canConvert.Should().BeFalse();
		result.Should()
			.ThrowExactly<InvalidCastException>()
			.WithMessage("The type 'System.IO.MemoryStream' does not implement IFormattable interface");
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateFormattableData))]
	public void ConvertToString_Formattable_ReturnsString(object input, string? format, string expected)
	{
		// Arrange
		var sut = new TestConverter(format, null);

		// Act
		var canConvert = sut.CanConvert(input.GetType());
		var result = sut.ConvertToString(input);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateCultureData))]
	public void ConvertToString_Formattable_ByCulture_ReturnsString(object input, CultureInfo? culture, string expected)
	{
		// Arrange
		var sut = new TestConverter(null, culture);

		// Act
		var canConvert = sut.CanConvert(input.GetType());
		var result = sut.ConvertToString(input);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}
	#endregion

	/// <inheritdoc cref="FormattableConverterBase"/>
	private sealed class TestConverter : FormattableConverterBase
	{
		public TestConverter(in string? format, IFormatProvider? formatProvider) : base(format, formatProvider) { }

		/// <inheritdoc cref="FormattableConverterBase.ConvertToString"/>
		public new string? ConvertToString(in object value) => base.ConvertToString(in value);
	}
}
