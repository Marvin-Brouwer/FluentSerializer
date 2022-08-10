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
/// Basically test if this converter behaves exactly like <see cref="TimeOnly.ToString()"/>
/// and <see cref="TimeOnly.Parse(string, IFormatProvider?)"/>
/// </summary>
public sealed class DefaultTimeOnlyConverterTests
{
	private static readonly string TimeOnlyString = "04:20:00";
	private static readonly TimeOnly TimeOnlyValue = TimeOnly.Parse(TimeOnlyString, CultureInfo.InvariantCulture);

	private readonly Mock<ISerializerContext<IJsonNode>> _contextMock;

	public DefaultTimeOnlyConverterTests()
	{
		var serializerMock = new Mock<IAdvancedJsonSerializer>()
			.UseConfig(JsonSerializerConfiguration.Default);
		_contextMock = new Mock<ISerializerContext<IJsonNode>>()
			.SetupDefault(serializerMock);
	}

	public static IEnumerable<object[]> GenerateConvertibleData()
	{
		yield return new object[] { "\"04:20:00\"", CultureInfo.InvariantCulture };
		yield return new object[] { "\"4:20 AM\"", new CultureInfo("en-US") };
		yield return new object[] { "\"04:20\"", new CultureInfo("nl-NL") };
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
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateCultureOptions))]
	public void SerializePattern_ReturnsString(CultureInfo cultureInfo)
	{
		// Arrange
		CultureInfo.CurrentCulture = cultureInfo;
		var expected = Value($"\"{TimeOnlyString}\"");
		var sut = new DefaultTimeOnlyConverter();

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
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateConvertibleData))]
	public void Deserialize_Convertible_ReturnsValue(string inputValue, CultureInfo cultureInfo)
	{
		// Arrange
		CultureInfo.CurrentCulture = cultureInfo;
		var input = Value(inputValue);
		var sut = new DefaultTimeOnlyConverter();

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
		Trait("Category", "UnitTest"),	Trait("DataFormat", "JSON")]
	public void Deserialize_Convertible_IncorrectFormat_Throws()
	{
		// Arrange
		var input = Value("SomeText");
		var sut = new DefaultTimeOnlyConverter();

		_contextMock
			.WithPropertyType(typeof(int));

		// Act
		var result = () => (TimeOnly?)sut.Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("String 'SomeText' was not recognized as a valid TimeOnly.");
	}
	#endregion
}

