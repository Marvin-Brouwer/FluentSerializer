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
/// Basically test if this converter behaves exactly like <see cref="DateOnly.ToString()"/>
/// and <see cref="DateOnly.Parse(string, IFormatProvider?)"/>
/// </summary>
public sealed class DefaultDateOnlyConverterTests
{
	private static readonly string DateOnlyString = "2096-04-20";
	private static readonly DateOnly DateOnlyValue = DateOnly.Parse(DateOnlyString, CultureInfo.InvariantCulture);

	private readonly Mock<ISerializerContext<IJsonNode>> _contextMock;

	public DefaultDateOnlyConverterTests()
	{
		var serializerMock = new Mock<IAdvancedJsonSerializer>()
			.UseConfig(JsonSerializerConfiguration.Default);
		_contextMock = new Mock<ISerializerContext<IJsonNode>>()
			.SetupDefault(serializerMock);
	}

	private static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { "\"2096-04-20\"", CultureInfo.InvariantCulture };
		yield return new object[] { "\"4/20/2096\"", new CultureInfo("en-US") };
		yield return new object[] { "\"20-04-2096\"", new CultureInfo("nl-NL") };
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
		var expected = Value($"\"{DateOnlyString}\"");
		var sut = new DefaultDateOnlyConverter();

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
	public void Deserialize_Convertible_ReturnsValue(string inputValue, CultureInfo cultureInfo)
	{
		// Arrange
		CultureInfo.CurrentCulture = cultureInfo;
		var input = Value(inputValue);
		var sut = new DefaultDateOnlyConverter();

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
		var sut = new DefaultDateOnlyConverter();

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = () => (DateOnly?)sut.Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("String 'SomeText' was not recognized as a valid DateOnly.");
	}
	#endregion
}

