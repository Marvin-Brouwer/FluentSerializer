using FluentAssertions;

using FluentSerializer.Core.Context;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Json.Configuration;
using FluentSerializer.Json.Converting.Converters;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Services;

using Moq;

using System;
using System.Collections.Generic;
using System.Globalization;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.Converting.Converters;

/// <summary>
/// Basically test if this converter behaves exactly like <see cref="DateOnly.ToString(string?)"/>
/// and <see cref="DateOnly.ParseExact(string, string, IFormatProvider?, DateTimeStyles)"/>
/// </summary>
public sealed class DateOnlyByFormatConverterTests
{
	private static readonly DateOnly DateOnlyValue = DateOnly.Parse("2096-04-20", CultureInfo.InvariantCulture);
	
	private readonly Mock<ISerializerContext<IJsonNode>> _contextMock;

	public DateOnlyByFormatConverterTests()
	{
		var serializerMock = new Mock<IAdvancedJsonSerializer>()
			.UseConfig(JsonSerializerConfiguration.Default);
		_contextMock = new Mock<ISerializerContext<IJsonNode>>()
			.SetupDefault(serializerMock);
	}

	public static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { "yyyy-MM-dd", "\"2096-04-20\"", CultureInfo.InvariantCulture };
		yield return new object[] { "M/d/yyyy", "\"4/20/2096\"", new CultureInfo("en-US") };
		yield return new object[] { "dd-MM-yyyy", "\"20-04-2096\"", new CultureInfo("nl-NL") };
	}

	#region Initialization

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Initialize_NullValues_Throws()
	{
		// Arrange
		var format = "g";
		var cultureInfo = CultureInfo.InvariantCulture;
		var dateTimeStyle = DateTimeStyles.AllowWhiteSpaces;

		// Act
		var result1 = () => new DateOnlyByFormatConverter(null!, cultureInfo, dateTimeStyle);
		var result2 = () => new DateOnlyByFormatConverter(format, null!, dateTimeStyle);

		// Assert
		result1.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(format));
		result2.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(cultureInfo));
	}

	#endregion

	#region Serialize

	[Theory,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateConvertibleData))]
	public void SerializePattern_ReturnsString(string pattern, string expectedValue, CultureInfo cultureInfo)
	{
		// Arrange
		var expected = Value(expectedValue);
		var sut = new DateOnlyByFormatConverter(pattern, cultureInfo, DateTimeStyles.AllowWhiteSpaces);

		// Act
		var canConvert = sut.CanConvert(DateOnlyValue.GetType());
		var result = sut.Serialize(DateOnlyValue, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquatableTo(expected);
	}

	#endregion

	#region Deserialize
	[Theory,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateConvertibleData))]
	public void Deserialize_Convertible_ReturnsValue(string pattern, string inputValue, CultureInfo cultureInfo)
	{
		// Arrange
		var input = Value(inputValue);
		var sut = new DateOnlyByFormatConverter(pattern, cultureInfo, DateTimeStyles.AllowWhiteSpaces);

		_contextMock
			.WithPropertyType(DateOnlyValue.GetType());

		// Act
		var canConvert = sut.CanConvert(DateOnlyValue.GetType());
		var result = (DateOnly)sut.Deserialize(input, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();
		result.Should().Be(DateOnlyValue);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void Deserialize_Convertible_IncorrectFormat_Throws()
	{
		// Arrange
		var input = Value("SomeText");
		var sut = new DateOnlyByFormatConverter("g", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces);

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = () => (DateOnly?)sut.Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("String 'SomeText' was not recognized as a valid DateOnly.");
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Deserialize_Convertible_EmptyString_Throws()
	{
		// Arrange
		var input = Value("\"\"");
		var sut = new DateOnlyByFormatConverter("g", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces);

		_contextMock
			.WithPropertyType(typeof(string));

		// Act
		var result = () => (DateOnly?)sut.Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("String '' was not recognized as a valid DateOnly.");
	}

	#endregion
}

