using FluentAssertions;

using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Xml.Converting.Converters;
using FluentSerializer.Xml.Tests.Extensions;

using System.Globalization;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.Converting.Converters;

public sealed partial class EnumConverterTests
{
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Attribute_ValueNull_ReturnsNull()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseDescription);

		// Act
		var result = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), null), _contextMock.Object);

		// Assert
		result.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Attribute_FormatUseEnumMember_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseEnumMember);

		// Act
		var result1 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithEnumMemberDataValue), _contextMock.Object);
		var result2 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithEnumMember);
		result2.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Attribute_FormatUseDescription_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseDescription);

		// Act
		var result1 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithDescriptionDataValue), _contextMock.Object);
		var result2 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result2.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Attribute_FormatUseName_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseName);

		// Act
		var result1 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithDescriptionName), _contextMock.Object);
		var result2 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithDescriptionDataValue), _contextMock.Object);
		var result3 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result2.Should().BeNull();
		result3.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Attribute_FormatUseNumberValue_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseNumberValue);

		// Act
		var result1 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);
		var result2 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithDescriptionDataValue), _contextMock.Object);
		var result3 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithoutDescriptionOrEnumMember);
		result2.Should().BeNull();
		result3.Should().BeEquivalentTo(TestValue.MemberWithExplicitValue);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Attribute_FormatUseEnumMemberOrUseName_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseEnumMember | EnumFormats.UseName);

		// Act
		var result1 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithEnumMemberDataValue), _contextMock.Object);
		var result2 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithEnumMemberName), _contextMock.Object);
		var result3 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithEnumMemberValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithEnumMember);
		result2.Should().BeEquivalentTo(TestValue.MemberWithEnumMember);
		result3.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Attribute_FormatUseDescriptionOrUseName_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseDescription | EnumFormats.UseName);

		// Act
		var result1 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithDescriptionDataValue), _contextMock.Object);
		var result2 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithDescriptionName), _contextMock.Object);
		var result3 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result2.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result3.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Attribute_FormatUseEnumMemberOrUseNumberValue_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseEnumMember | EnumFormats.UseNumberValue);

		// Act
		var result1 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithEnumMemberDataValue), _contextMock.Object);
		var result2 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithEnumMemberName), _contextMock.Object);
		var result3 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithEnumMember);
		result2.Should().BeNull();
		result3.Should().BeEquivalentTo(TestValue.MemberWithExplicitValue);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Attribute_FormatUseDescriptionOrUseNumberValue_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseDescription | EnumFormats.UseNumberValue);

		// Act
		var result1 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithDescriptionDataValue), _contextMock.Object);
		var result2 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithDescriptionName), _contextMock.Object);
		var result3 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result2.Should().BeNull();
		result3.Should().BeEquivalentTo(TestValue.MemberWithExplicitValue);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Attribute_FormatUseNameOrUseNumberValue_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseName | EnumFormats.UseNumberValue);

		// Act
		var result1 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithDescriptionDataValue), _contextMock.Object);
		var result2 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithDescriptionName), _contextMock.Object);
		var result3 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);
		var result4 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithExplicitValueName), _contextMock.Object);

		// Assert
		result1.Should().BeNull();
		result2.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result3.Should().BeEquivalentTo(TestValue.MemberWithExplicitValue);
		result4.Should().BeEquivalentTo(TestValue.MemberWithExplicitValue);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Deserialize_Attribute_FormatUseAll_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseEnumMember | EnumFormats.UseDescription | EnumFormats.UseName | EnumFormats.UseNumberValue);

		// Act
		var result1 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithEnumMemberDataValue), _contextMock.Object);
		var result2 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithDescriptionDataValue), _contextMock.Object);
		var result3 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithDescriptionName), _contextMock.Object);
		var result4 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture)), _contextMock.Object);
		var result5 = sut.ForAttribute().Deserialize(Attribute(nameof(Attribute), MemberWithEnumMemberAndDescriptionDataValueMember), _contextMock.Object);

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithEnumMember);
		result2.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result3.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result4.Should().BeEquivalentTo(TestValue.MemberWithExplicitValue);
		result5.Should().BeEquivalentTo(TestValue.MemberWithEnumMemberAndDescription);
	}
}
