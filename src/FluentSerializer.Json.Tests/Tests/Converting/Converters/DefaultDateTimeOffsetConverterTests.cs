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
/// Basically test if this converter behaves exactly like <see cref="DateTimeOffset.ToString()"/>
/// and <see cref="DateTimeOffset.Parse(string, IFormatProvider?)"/>
/// </summary>
public sealed class DefaultDateTimeOffsetConverterTests
{
	private static readonly string DateTimeOffsetString = "2096-04-20T04:20:00+00:00";
	private static readonly DateTimeOffset DateTimeOffsetValue = DateTimeOffset.Parse(
		DateTimeOffsetString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

	private readonly Mock<ISerializerContext<IJsonNode>> _contextMock;

	public DefaultDateTimeOffsetConverterTests()
	{
		var serializerMock = new Mock<IAdvancedJsonSerializer>()
			.UseConfig(JsonSerializerConfiguration.Default);
		_contextMock = new Mock<ISerializerContext<IJsonNode>>()
			.SetupDefault(serializerMock);
	}

	public static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { true, "\"2096-04-20 04:20:00 +0\"", CultureInfo.InvariantCulture };
		yield return new object[] { false, "\"4/20/2096 +0\"", new CultureInfo("en-US") };
		yield return new object[] { true, "\"4/20/2096 4:20 AM +0\"", new CultureInfo("en-US") };
		yield return new object[] { true, "\"20-04-2096 04:20 +0\"", new CultureInfo("nl-NL") };
	}

	public static IEnumerable<object[]> GenerateCultureOptions()
	{
		yield return new object[] { CultureInfo.InvariantCulture };
		yield return new object[] { new CultureInfo("en") };
		yield return new object[] { new CultureInfo("en-US") };
		yield return new object[] { new CultureInfo("nl-NL") };
	}

	#region Serialize

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateCultureOptions))]
	public void SerializePattern_ReturnsString(CultureInfo cultureInfo)
	{
		// Arrange
		CultureInfo.CurrentCulture = cultureInfo;
		var expected = Value($"\"{DateTimeOffsetString}\"");
		var sut = new DefaultDateTimeOffsetConverter();

		// Act
		var canConvert = sut.CanConvert(DateTimeOffsetValue.GetType());
		var result = sut.Serialize(DateTimeOffsetValue, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquatableTo(expected);
	}
	#endregion

	#region Deserialize
	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateConvertibleData))]
	public void Deserialize_Convertible_ReturnsValue(bool hasTime, string inputValue, CultureInfo cultureInfo)
	{
		// Arrange
		CultureInfo.CurrentCulture = cultureInfo;
		var input = Value(inputValue);
		var sut = new DefaultDateTimeOffsetConverter();

		_contextMock
			.WithPropertyType(DateTimeOffsetValue.GetType());

		// Act
		var canConvert = sut.CanConvert(DateTimeOffsetValue.GetType());
		var result = (DateTimeOffset)sut.Deserialize(input, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();
		if (!hasTime) result.Should().BeSameDateAs(DateTimeOffsetValue);
		else result.Should().Be(DateTimeOffsetValue);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Deserialize_Convertible_IncorrectFormat_Throws()
	{
		// Arrange
		var input = Value("SomeText");
		var sut = new DefaultDateTimeOffsetConverter();

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = () => (DateTimeOffset?)sut.Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("The string 'SomeText' was not recognized as a valid DateTime. There is an unknown word starting at index '0'.");
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Deserialize_Convertible_EmptyString_Throws()
	{
		// Arrange
		var input = Value("\"\"");
		var sut = new DefaultDateTimeOffsetConverter();

		_contextMock
			.WithPropertyType(typeof(string));

		// Act
		var result = () => (DateOnly?)sut.Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("String '' was not recognized as a valid DateTime.");
	}

	#endregion
}

