using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using FluentSerializer.Core.Converting.Converters;
using Xunit;
using System.Diagnostics.CodeAnalysis;

namespace FluentSerializer.Core.Tests.Tests.Converting.Converters;

/// <summary>
/// Basically test if this converter behaves exactly like <see cref="Convert.ToString(bool)"/>
/// and <see cref="Convert.ChangeType(object?, Type)"/>
/// </summary>
public sealed class ConvertibleConverterBaseTests
{
	private readonly TestConverter _sut;

	public ConvertibleConverterBaseTests()
	{
		_sut = new TestConverter();
	}

	public static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { 1, "1" };
		yield return new object[] { true, "True" };
		// JSON strings need to be wrapped in quotes
		yield return new object[] { "string", "string" };
	}

	#region ConvertToString
	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		InlineData(null), InlineData("")]
	public void ConvertToString_NullOrEmpty_ReturnsEmptyString(string input)
	{
		// Arrange
		var expected = string.Empty;

		// Act
		var canConvert = _sut.CanConvert(typeof(string));
		var result = _sut.ConvertToString(input);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void ConvertToString_NonConvertible_ReturnsToString()
	{
		// Arrange
		using var input = new MemoryStream(0);
		var expected = input.ToString();

		// Act
		var canConvert = _sut.CanConvert(input.GetType());
		var result = _sut.ConvertToString(input);

		// Assert
		canConvert.Should().BeFalse();
		result.Should().BeEquivalentTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateConvertibleData))]
	public void ConvertToStringAttributeConvertible_ReturnsString(object input, string expected)
	{
		// Act
		var canConvert = _sut.CanConvert(input.GetType());
		var result = _sut.ConvertToString(input);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}
	#endregion

	#region ConvertToNullableDataType
	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateConvertibleData))]
	public void ConvertToNullableDataType_EmptyValue_ReturnsDefault(object requested, string unused)
	{
		_ = unused;

		// Arrange
		var input = string.Empty;
		var expected = (object?)null;

		// Act
		var canConvert = _sut.CanConvert(requested.GetType());
		var result = _sut.ConvertToNullableDataType(input, typeof(bool?));

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateConvertibleData))]
	public void ConvertToNullableDataType_Convertible_ReturnsValue(object expected, string input)
	{
		// Act
		var canConvert = _sut.CanConvert(expected.GetType());
		var result = _sut.ConvertToNullableDataType(input, expected.GetType());

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void ConvertToNullableDataType_Convertible_IncorrectFormat_Throws()
	{
		// Arrange
		var input = "SomeText";

		// Act
		var result = () => _sut.ConvertToNullableDataType(input, typeof(int));

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("Input string was not in a correct format.");
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void ConvertToNullableDataType_NonConvertible_Throws()
	{
		// Arrange
		var input = "Doesn't matter";

		// Act
		var canConvert = _sut.CanConvert(typeof(Stream));
		var result = () => _sut.ConvertToNullableDataType(input, typeof(Stream));

		// Assert
		canConvert.Should().BeFalse();
		result.Should()
			.ThrowExactly<InvalidCastException>()
			.WithMessage("Invalid cast from 'System.String' to 'System.IO.Stream'.");
	}
	#endregion

	/// <inheritdoc cref="ConvertibleConverterBase"/>
	[SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "TestImplementation")]
	private sealed class TestConverter : ConvertibleConverterBase
	{
		/// <inheritdoc cref="ConvertibleConverterBase.ConvertToString"/>
		public new string? ConvertToString(in object value) => ConvertibleConverterBase.ConvertToString(in value);

		/// <inheritdoc cref="ConvertibleConverterBase.ConvertToNullableDataType"/>
		public new object? ConvertToNullableDataType(in string? currentValue, in Type targetType) =>
			ConvertibleConverterBase.ConvertToNullableDataType(in currentValue, in targetType);
	}

}

