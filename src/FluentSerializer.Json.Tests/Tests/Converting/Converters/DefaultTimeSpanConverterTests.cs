using FluentAssertions;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Core.TestUtils.Extensions;
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
/// Basically test if this converter behaves exactly like <see cref="TimeSpan.ToString()"/>
/// and <see cref="TimeSpan.Parse(string, IFormatProvider?)"/>
/// </summary>
public sealed class DefaultTimeSpanConverterTests
{
	private static readonly string TimeSpanString = "04:20:00";
	private static readonly TimeSpan TimeSpanValue = TimeSpan.Parse(TimeSpanString, CultureInfo.InvariantCulture);

	private readonly Mock<ISerializerContext<IJsonNode>> _contextMock;

	public DefaultTimeSpanConverterTests()
	{
		var serializerMock = new Mock<IAdvancedJsonSerializer>()
			.SetupDefault();
		_contextMock = new Mock<ISerializerContext<IJsonNode>>()
			.SetupDefault(serializerMock);
	}

	private static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { "\"04:20:00\"" };
		yield return new object[] { "\"04:20\"" };
	}

	#region Serialize

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void SerializePattern_ReturnsString()
	{
		// Arrange
		var expected = Value($"\"{TimeSpanString}\"");
		var sut = new DefaultTimeSpanConverter();

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
	public void Deserialize_Convertible_ReturnsValue(string inputValue)
	{
		// Arrange
		var input = Value(inputValue);
		var sut = new DefaultTimeSpanConverter();

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
		var sut = new DefaultTimeSpanConverter();

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = () => (TimeSpan?)sut.Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("String 'SomeText' was not recognized as a valid TimeSpan.");
	}
	#endregion
}

