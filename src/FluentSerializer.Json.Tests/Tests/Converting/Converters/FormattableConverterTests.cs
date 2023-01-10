using FluentAssertions;

using FluentSerializer.Core.Context;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Json.Configuration;
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
/// Basically test if this converter behaves exactly like <see cref="IFormattable"/>
/// </summary>
public sealed class FormattableConverterTests
{
	private readonly FormattableConverter _sut;
	private readonly Mock<ISerializerContext<IJsonNode>> _contextMock;

	public FormattableConverterTests()
	{
		_sut = new FormattableConverter(null, new CultureInfo("nl-NL"));

		var serializerMock = new Mock<IAdvancedJsonSerializer>()
			.UseConfig(JsonSerializerConfiguration.Default);
		_contextMock = new Mock<ISerializerContext<IJsonNode>>()
			.SetupDefault(serializerMock);
	}

	public static IEnumerable<object[]> GenerateFormattibleData()
	{
		yield return new object[] { 1, "1" };
		yield return new object[] { new DateOnly(1991, 11, 28), "28-11-1991" };
		yield return new object[] { new TimeOnly(12, 00, 00), "12:00" };
		yield return new object[] { 6.9, "6,9" };
	}

	#region Serialize

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Serialize_NonFormattible_Throws()
	{
		// Arrange
		using var input = new MemoryStream(0);

		// Act
		var canConvert = _sut.CanConvert(input.GetType());
		var result = () => _sut.Serialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeFalse();
		result.Should()
			.ThrowExactly<InvalidCastException>()
			.WithMessage("The type 'System.IO.MemoryStream' does not implement IFormattable interface");
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON"),
		MemberData(nameof(GenerateFormattibleData))]
	public void Serialize_Formattible_ReturnsString(object input, string expectedValue)
	{
		// Arrange
		var expected = Value(expectedValue);

		// Act
		var canConvert = _sut.CanConvert(input.GetType());
		var result = _sut.Serialize(input, _contextMock.Object);

		// Assert
		canConvert.Should().BeTrue();
		result.Should().BeEquivalentTo(expected);
	}
	#endregion

	#region Deserialize

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Deserialize_ThrowsNotSupported()
	{
		// Arrange
		var input = Value("Doesn't matter");

		// Act
		var result = () => _sut.Deserialize(input, _contextMock.Object);

		// Assert
		result.Should()
			.ThrowExactly<NotSupportedException>();
	}

	#endregion
}
