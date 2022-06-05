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
/// Basically test if this converter behaves exactly like <see cref="TimeSpan.ToString(string?)"/>
/// and <see cref="TimeSpan.ParseExact(string, string, IFormatProvider?, DateTimeStyles)"/>
/// </summary>
public sealed class TimeSpanByFormatConverterTests
{
	private static readonly TimeSpan TimeSpanValue = TimeSpan.Parse("04:20:00", CultureInfo.InvariantCulture);
	
	private readonly Mock<ISerializerContext<IJsonNode>> _contextMock;

	public TimeSpanByFormatConverterTests()
	{
		var serializerMock = new Mock<IAdvancedJsonSerializer>()
			.UseConfig(JsonSerializerConfiguration.Default);
		_contextMock = new Mock<ISerializerContext<IJsonNode>>()
			.SetupDefault(serializerMock);
	}

	private static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { @"hh\:mm\:ss\.fff", "\"04:20:00.000\"" };
		yield return new object[] { @"hh\:mm\:ss", "\"04:20:00\"" };
		yield return new object[] { @"hh\:mm", "\"04:20\"" };
	}

	#region Serialize

	[Theory,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateConvertibleData))]
	public void SerializePattern_ReturnsString(string pattern, string expectedValue)
	{
		// Arrange
		var expected = Value(expectedValue);
		var sut = new TimeSpanByFormatConverter(pattern, CultureInfo.InvariantCulture, TimeSpanStyles.None);

		// Act
		var canConvert = sut.CanConvert(TimeSpanValue.GetType());
		var result = sut.Serialize(TimeSpanValue, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquatableTo(expected);
	}
	#endregion

	#region Deserialize
	[Theory,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateConvertibleData))]
	public void Deserialize_Convertible_ReturnsValue(string pattern, string inputValue)
	{
		// Arrange
		var input = Value(inputValue);
		var sut = new TimeSpanByFormatConverter(pattern, CultureInfo.InvariantCulture, TimeSpanStyles.None);

		_contextMock
			.WithPropertyType(TimeSpanValue.GetType());

		// Act
		var canConvert = sut.CanConvert(TimeSpanValue.GetType());
		var result = (TimeSpan)sut.Deserialize(input, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();
		result.Should().Be(TimeSpanValue);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void Deserialize_Convertible_IncorrectFormat_Throws()
	{
		// Arrange
		var input = Value("SomeText");
		var sut = new TimeSpanByFormatConverter("HH:mm:ss", CultureInfo.InvariantCulture, TimeSpanStyles.None);

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = () => (TimeSpan?)sut.Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("Input string was not in a correct format.");
	}
	#endregion
}

