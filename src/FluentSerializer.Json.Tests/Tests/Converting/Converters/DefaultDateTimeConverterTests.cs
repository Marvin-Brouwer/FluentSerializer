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
/// Basically test if this converter behaves exactly like <see cref="DateTime.ToString()"/>
/// and <see cref="DateTime.Parse(string, IFormatProvider?)"/>
/// </summary>
public sealed class DefaultDateTimeConverterTests
{
	private static readonly string DateTimeString = "2096-04-20T04:20:00Z";
	private static readonly DateTime DateTimeValue = DateTime.Parse(
		DateTimeString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

	private readonly Mock<ISerializerContext<IJsonNode>> _contextMock;

	public DefaultDateTimeConverterTests()
	{
		var serializerMock = new Mock<IAdvancedJsonSerializer>();
		_contextMock = new Mock<ISerializerContext<IJsonNode>>()
			.SetupDefault(serializerMock);
	}

	private static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { true, "\"2096-04-20 04:20:00\"", CultureInfo.InvariantCulture };
		yield return new object[] { false, "\"4/20/2096\"", new CultureInfo("en-US") };
		yield return new object[] { true, "\"4/20/2096 4:20 AM\"", new CultureInfo("en-US") };
		yield return new object[] { true, "\"20-04-2096 04:20\"", new CultureInfo("nl-NL") };
	}

	private static IEnumerable<object[]> GenerateCultureOptions()
	{
		yield return new object[] { CultureInfo.InvariantCulture };
		yield return new object[] { new CultureInfo("en") };
		yield return new object[] { new CultureInfo("en-US") };
		yield return new object[] { new CultureInfo("nl-NL") };
	}

	#region Serialize

	[Theory,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateCultureOptions))]
	public void SerializePattern_ReturnsString(CultureInfo cultureInfo)
	{
		// Arrange
		CultureInfo.CurrentCulture = cultureInfo;
		var expected = Value($"\"{DateTimeString}\"");
		var sut = new DefaultDateTimeConverter();

		// Act
		var canConvert = sut.CanConvert(DateTimeValue.GetType());
		var result = sut.Serialize(DateTimeValue, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquatableTo(expected);
	}
	#endregion

	#region Deserialize
	[Theory,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateConvertibleData))]
	public void Deserialize_Convertible_ReturnsValue(bool hasTime, string inputValue, CultureInfo cultureInfo)
	{
		// Arrange
		CultureInfo.CurrentCulture = cultureInfo;
		var input = Value(inputValue);
		var sut = new DefaultDateTimeConverter();

		_contextMock
			.WithPropertyType(DateTimeValue.GetType());

		// Act
		var canConvert = sut.CanConvert(DateTimeValue.GetType());
		var result = (DateTime)sut.Deserialize(input, _contextMock.Object)!;

		// Assert
		canConvert.Should().BeTrue();
		if (!hasTime) result.Should().BeSameDateAs(DateTimeValue);
		else result.Should().Be(DateTimeValue);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void Deserialize_Convertible_IncorrectFormat_Throws()
	{
		// Arrange
		var input = Value("SomeText");
		var sut = new DefaultDateTimeConverter();

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = () => (DateTime?)sut.Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("The string 'SomeText' was not recognized as a valid DateTime. There is an unknown word starting at index '0'.");
	}
	#endregion
}

