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
/// Basically test if this converter behaves exactly like <see cref="TimeOnly.ToString(string?)"/>
/// and <see cref="TimeOnly.ParseExact(string, string, IFormatProvider?, DateTimeStyles)"/>
/// </summary>
public sealed class TimeOnlyByFormatConverterTests
{
	private static readonly TimeOnly TimeOnlyValue = TimeOnly.Parse("04:20:00", CultureInfo.InvariantCulture);

	private readonly Mock<ISerializerContext<IJsonNode>> _contextMock;

	public TimeOnlyByFormatConverterTests()
	{
		var serializerMock = new Mock<IAdvancedJsonSerializer>()
			.UseConfig(JsonSerializerConfiguration.Default);
		_contextMock = new Mock<ISerializerContext<IJsonNode>>()
			.SetupDefault(serializerMock);
	}

	public static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { "HH:mm:ss", "\"04:20:00\"", CultureInfo.InvariantCulture };
		yield return new object[] { "h:mm tt", "\"4:20 AM\"", new CultureInfo("en-US") };
		yield return new object[] { "HH:mm", "\"04:20\"", new CultureInfo("nl-NL") };
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
		var result1 = () => new TimeOnlyByFormatConverter(null!, cultureInfo, dateTimeStyle);
		var result2 = () => new TimeOnlyByFormatConverter(format, null!, dateTimeStyle);

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
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateConvertibleData))]
	public void SerializePattern_ReturnsString(string pattern, string expectedValue, CultureInfo cultureInfo)
	{
		// Arrange
		var expected = Value(expectedValue);
		var sut = new TimeOnlyByFormatConverter(pattern, cultureInfo, DateTimeStyles.AllowWhiteSpaces);

		// Act
		var canConvert = sut.CanConvert(TimeOnlyValue.GetType());
		var result = sut.Serialize(TimeOnlyValue, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquatableTo(expected);
	}
	#endregion

	#region Deserialize
	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateConvertibleData))]
	public void Deserialize_Convertible_ReturnsValue(string pattern, string inputValue, CultureInfo cultureInfo)
	{
		// Arrange
		var input = Value(inputValue);
		var sut = new TimeOnlyByFormatConverter(pattern, cultureInfo, DateTimeStyles.AllowWhiteSpaces);

		_contextMock
			.WithPropertyType(TimeOnlyValue.GetType());

		// Act
		var canConvert = sut.CanConvert(TimeOnlyValue.GetType());
		var result = (TimeOnly)sut.Deserialize(input, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();
		result.Should().Be(TimeOnlyValue);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Deserialize_Convertible_IncorrectFormat_Throws()
	{
		// Arrange
		var input = Value("SomeText");
		var sut = new TimeOnlyByFormatConverter("HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces);

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = () => (TimeOnly?)sut.Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("String 'SomeText' was not recognized as a valid TimeOnly.");
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Deserialize_Convertible_EmptyString_Throws()
	{
		// Arrange
		var input = Value("\"\"");
		var sut = new TimeOnlyByFormatConverter("g", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces);

		_contextMock
			.WithPropertyType(typeof(string));

		// Act
		var result = () => (DateOnly?)sut.Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("String '' was not recognized as a valid TimeOnly.");
	}

	#endregion
}

