using FluentAssertions;

using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Json.Converting.Converters;

using System.Globalization;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.Converting.Converters;

public sealed partial class EnumConverterTests
{
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_ValueNull_ReturnsNull()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseDescription, false);

		// Act
		var result = sut.Deserialize(Value(null), _contextMock.Object);

		// Assert
		result.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_FormatUseEnumMember_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseEnumMember, false);

		// Act
		var result1 = sut.Deserialize(Value(MemberWithEnumMemberDataValue), _contextMock.Object);
		var result2 = sut.Deserialize(Value(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithEnumMember);
		result2.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_FormatUseDescription_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseDescription, false);

		// Act
		var result1 = sut.Deserialize(Value(MemberWithDescriptionDataValue), _contextMock.Object);
		var result2 = sut.Deserialize(Value(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result2.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_FormatUseName_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseName, false);

		// Act
		var result1 = sut.Deserialize(Value(MemberWithDescriptionName), _contextMock.Object);
		var result2 = sut.Deserialize(Value(MemberWithDescriptionDataValue), _contextMock.Object);
		var result3 = sut.Deserialize(Value(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result2.Should().BeNull();
		result3.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_FormatUseNumberValue_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseNumberValue, false);

		// Act
		var result1 = sut.Deserialize(Value(MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);
		var result2 = sut.Deserialize(Value(MemberWithDescriptionDataValue), _contextMock.Object);
		var result3 = sut.Deserialize(Value(MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithoutDescriptionOrEnumMember);
		result2.Should().BeNull();
		result3.Should().BeEquivalentTo(TestValue.MemberWithExplicitValue);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_FormatUseEnumMemberOrUseName_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseEnumMember | EnumFormats.UseName, false);

		// Act
		var result1 = sut.Deserialize(Value(MemberWithEnumMemberDataValue), _contextMock.Object);
		var result2 = sut.Deserialize(Value(MemberWithEnumMemberName), _contextMock.Object);
		var result3 = sut.Deserialize(Value(MemberWithEnumMemberValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithEnumMember);
		result2.Should().BeEquivalentTo(TestValue.MemberWithEnumMember);
		result3.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_FormatUseDescriptionOrUseName_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseDescription | EnumFormats.UseName, false);

		// Act
		var result1 = sut.Deserialize(Value(MemberWithDescriptionDataValue), _contextMock.Object);
		var result2 = sut.Deserialize(Value(MemberWithDescriptionName), _contextMock.Object);
		var result3 = sut.Deserialize(Value(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result2.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result3.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_FormatUseEnumMemberOrUseNumberValue_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseEnumMember | EnumFormats.UseNumberValue, false);

		// Act
		var result1 = sut.Deserialize(Value(MemberWithEnumMemberDataValue), _contextMock.Object);
		var result2 = sut.Deserialize(Value(MemberWithEnumMemberName), _contextMock.Object);
		var result3 = sut.Deserialize(Value(MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithEnumMember);
		result2.Should().BeNull();
		result3.Should().BeEquivalentTo(TestValue.MemberWithExplicitValue);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_FormatUseDescriptionOrUseNumberValue_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseDescription | EnumFormats.UseNumberValue, false);

		// Act
		var result1 = sut.Deserialize(Value(MemberWithDescriptionDataValue), _contextMock.Object);
		var result2 = sut.Deserialize(Value(MemberWithDescriptionName), _contextMock.Object);
		var result3 = sut.Deserialize(Value(MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result2.Should().BeNull();
		result3.Should().BeEquivalentTo(TestValue.MemberWithExplicitValue);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_FormatUseNameOrUseNumberValue_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseName | EnumFormats.UseNumberValue, false);

		// Act
		var result1 = sut.Deserialize(Value(MemberWithDescriptionDataValue), _contextMock.Object);
		var result2 = sut.Deserialize(Value(MemberWithDescriptionName), _contextMock.Object);
		var result3 = sut.Deserialize(Value(MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);
		var result4 = sut.Deserialize(Value(MemberWithExplicitValueName), _contextMock.Object);

		// Assert
		result1.Should().BeNull();
		result2.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result3.Should().BeEquivalentTo(TestValue.MemberWithExplicitValue);
		result4.Should().BeEquivalentTo(TestValue.MemberWithExplicitValue);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_FormatUseAll_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseEnumMember | EnumFormats.UseDescription | EnumFormats.UseName | EnumFormats.UseNumberValue, false);

		// Act
		var result1 = sut.Deserialize(Value(MemberWithEnumMemberDataValue), _contextMock.Object);
		var result2 = sut.Deserialize(Value(MemberWithDescriptionDataValue), _contextMock.Object);
		var result3 = sut.Deserialize(Value(MemberWithDescriptionName), _contextMock.Object);
		var result4 = sut.Deserialize(Value(MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);
		var result5 = sut.Deserialize(Value(MemberWithEnumMemberAndDescriptionDataValueMember), _contextMock.Object);

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithEnumMember);
		result2.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result3.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result4.Should().BeEquivalentTo(TestValue.MemberWithExplicitValue);
		result5.Should().BeEquivalentTo(TestValue.MemberWithEnumMemberAndDescription);
	}
}
