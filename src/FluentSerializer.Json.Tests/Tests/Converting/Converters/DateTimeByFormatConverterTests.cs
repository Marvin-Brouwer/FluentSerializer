using FluentAssertions;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Json.Converting.Converters;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.Converting.Converters;

/// <summary>
/// Basically test if this converter behaves exactly like <see cref="DateTime.ToString(string?)"/>
/// and <see cref="DateTime.ParseExact(string, string, IFormatProvider?, DateTimeStyles)"/>
/// </summary>
public sealed class DateTimeByFormatConverterTests
{
	private static readonly DateTime DateTimeValue = DateTime.Parse(
		"2096-04-20T04:20:00Z", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
	
	private readonly Mock<ISerializerContext<IJsonNode>> _contextMock;

	public DateTimeByFormatConverterTests()
	{
		var serializerMock = new Mock<IAdvancedJsonSerializer>();
		_contextMock = new Mock<ISerializerContext<IJsonNode>>()
			.SetupDefault(serializerMock);
	}

	private static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { "yyyy-MM-dd HH:mm:ss", "\"2096-04-20 04:20:00\"", CultureInfo.InvariantCulture };
		yield return new object[] { "d", "\"4/20/2096\"", new CultureInfo("en-US") };
		yield return new object[] { "g", "\"4/20/2096 4:20 AM\"", new CultureInfo("en-US") };
		yield return new object[] { "g", "\"20-04-2096 04:20\"", new CultureInfo("nl-NL") };
	}

	#region Serialize

	[Theory,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateConvertibleData))]
	public void SerializePattern_ReturnsString(string pattern, string expectedValue, CultureInfo cultureInfo)
	{
		// Arrange
		var expected = Value(expectedValue);
		var sut = new DateTimeByFormatConverter(pattern, cultureInfo, DateTimeStyles.AssumeUniversal);

		// Act
		var canConvert = sut.CanConvert(DateTimeValue.GetType());
		var result = sut.Serialize(DateTimeValue, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
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
		var sut = new DateTimeByFormatConverter(pattern, cultureInfo, DateTimeStyles.None);

		_contextMock
			.WithPropertyType(DateTimeValue.GetType());

		// Act
		var canConvert = sut.CanConvert(DateTimeValue.GetType());
		var result = (DateTime)sut.Deserialize(input, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();
		if (pattern == "d") result.Should().BeSameDateAs(DateTimeValue);
		else result.Should().Be(DateTimeValue);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void Deserialize_Convertible_IncorrectFormat_Throws()
	{
		// Arrange
		var input = Value("SomeText");
		var sut = new DateTimeByFormatConverter("g", CultureInfo.InvariantCulture, DateTimeStyles.None);

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = () => (DateTime?)sut.Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("String 'SomeText' was not recognized as a valid DateTime.");
	}
	#endregion
}

