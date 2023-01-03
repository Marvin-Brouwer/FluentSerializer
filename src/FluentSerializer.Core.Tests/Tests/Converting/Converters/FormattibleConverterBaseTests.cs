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
	private readonly TestConverter _sut;

	public FormattibleConverterBaseTests()
	{
		_sut = new TestConverter();
		Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
	}

	public static IEnumerable<object[]> GenerateFormattableData()
	{
		yield return new object[] { 1, "1" };
		yield return new object[] { new DateOnly(1991, 11, 28), "28-11-1991" };
		yield return new object[] { new TimeOnly(12, 00, 00), "12:00" };
		yield return new object[] { 6.9, "6,9" };
	}

	#region ConvertToString

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void ConvertToString_NonFormattable_Throws()
	{
		// Arrange
		using var input = new MemoryStream(0);

		// Act
		var canConvert = _sut.CanConvert(input.GetType());
		var result = () => _sut.ConvertToString(input);

		// Assert
		canConvert.Should().BeFalse();
		result.Should()
			.ThrowExactly<InvalidCastException>()
			.WithMessage("The type 'System.IO.MemoryStream' does not implement IFormattable interface");
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateFormattableData))]
	public void ConvertToString_Formattable_ReturnsString(object input, string expected)
	{
		// Act
		var canConvert = _sut.CanConvert(input.GetType());
		var result = _sut.ConvertToString(input);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}
	#endregion

	/// <inheritdoc cref="FormattableConverterBase"/>
	private sealed class TestConverter : FormattableConverterBase
	{
		public TestConverter() : base(null) { }

		/// <inheritdoc cref="FormattableConverterBase.ConvertToString"/>
		public new string? ConvertToString(in object value) => base.ConvertToString(in value);
	}
}
