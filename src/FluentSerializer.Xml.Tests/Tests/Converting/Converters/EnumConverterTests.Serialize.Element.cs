using FluentAssertions;

using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Xml.Converting.Converters;
using FluentSerializer.Xml.Tests.Extensions;

using System;
using System.Globalization;

using Xunit;

namespace FluentSerializer.Xml.Tests.Tests.Converting.Converters;

public sealed partial class EnumConverterTests
{
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_Element_FormatUseEnumMember_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseEnumMember);

		// Act
		var result1 = () => sut.ForElement().Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = () => sut.ForElement().Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.ForElement().Serialize(TestValue.MemberWithEnumMember, _contextMock.Object);
		var result4 = sut.ForElement().Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1.Should().ThrowExactly<NotSupportedException>();
		result2.Should().ThrowExactly<NotSupportedException>();
		result3!.GetTextValue().Should().BeEquivalentTo(MemberWithEnumMemberDataValue);
		result3!.GetTextValue().Should().NotBeEquivalentTo(MemberWithEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result4!.GetTextValue().Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueMember);
		result4!.GetTextValue().Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionValue.ToString(CultureInfo.InvariantCulture));
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_Element_FormatUseDescription_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseDescription);

		// Act
		var result1 = sut.ForElement().Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = () => sut.ForElement().Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = () => sut.ForElement().Serialize(TestValue.MemberWithEnumMember, _contextMock.Object);
		var result4 = sut.ForElement().Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.GetTextValue().Should().BeEquivalentTo(MemberWithDescriptionDataValue);
		result1!.GetTextValue().Should().NotBeEquivalentTo(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture));
		result2.Should().ThrowExactly<NotSupportedException>();
		result3.Should().ThrowExactly<NotSupportedException>();
		result4!.GetTextValue().Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueDescription);
		result4!.GetTextValue().Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionValue.ToString(CultureInfo.InvariantCulture));
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_Element_FormatUseName_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseName);

		// Act
		var result1 = sut.ForElement().Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = sut.ForElement().Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.ForElement().Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.GetTextValue().Should().BeEquivalentTo(MemberWithDescriptionName);
		result1!.GetTextValue().Should().NotBeEquivalentTo(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture));
		result2!.GetTextValue().Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberName);
		result2!.GetTextValue().Should().NotBeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result3!.GetTextValue().Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionName);
		result3!.GetTextValue().Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionValue.ToString(CultureInfo.InvariantCulture));
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_Element_FormatUseNumberValue_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseNumberValue);

		// Act
		var result1 = sut.ForElement().Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = sut.ForElement().Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.ForElement().Serialize(TestValue.MemberWithExplicitValue, _contextMock.Object);
		var result4 = sut.ForElement().Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.GetTextValue().Should().BeEquivalentTo(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture));
		result1!.GetTextValue().Should().NotBeEquivalentTo(MemberWithDescriptionName);
		result1!.GetTextValue().Should().NotBeEquivalentTo(MemberWithEnumMemberDataValue);
		result2!.GetTextValue().Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result2!.GetTextValue().Should().NotBeEquivalentTo(MemberWithDescriptionName);
		result3!.GetTextValue().Should().BeEquivalentTo(MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture));
		result3!.GetTextValue().Should().NotBeEquivalentTo(MemberWithExplicitValueName);
		result4!.GetTextValue().Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionValue.ToString(CultureInfo.InvariantCulture));
		result4!.GetTextValue().Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionName);
		result4!.GetTextValue().Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueMember);
		result4!.GetTextValue().Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueDescription);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_Element_FormatUseEnumMemberOrUseName_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseEnumMember | EnumFormats.UseName);

		// Act
		var result1 = sut.ForElement().Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = sut.ForElement().Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.ForElement().Serialize(TestValue.MemberWithEnumMember, _contextMock.Object);
		var result4 = sut.ForElement().Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.GetTextValue().Should().BeEquivalentTo(MemberWithDescriptionName);
		result1!.GetTextValue().Should().NotBeEquivalentTo(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture));
		result2!.GetTextValue().Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberName);
		result2!.GetTextValue().Should().NotBeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result3!.GetTextValue().Should().BeEquivalentTo(MemberWithEnumMemberDataValue);
		result3!.GetTextValue().Should().NotBeEquivalentTo(MemberWithEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result4!.GetTextValue().Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueMember);
		result4!.GetTextValue().Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionValue.ToString(CultureInfo.InvariantCulture));
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_Element_FormatUseDescriptionOrUseName_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseDescription | EnumFormats.UseName);

		// Act
		var result1 = sut.ForElement().Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = sut.ForElement().Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.ForElement().Serialize(TestValue.MemberWithEnumMember, _contextMock.Object);
		var result4 = sut.ForElement().Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.GetTextValue().Should().BeEquivalentTo(MemberWithDescriptionDataValue);
		result1!.GetTextValue().Should().NotBeEquivalentTo(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture));
		result2!.GetTextValue().Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberName);
		result2!.GetTextValue().Should().NotBeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result3!.GetTextValue().Should().BeEquivalentTo(MemberWithEnumMemberName);
		result3!.GetTextValue().Should().NotBeEquivalentTo(MemberWithEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result4!.GetTextValue().Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueDescription);
		result4!.GetTextValue().Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionValue.ToString(CultureInfo.InvariantCulture));
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_Element_FormatUseEnumMemberOrUseNumberValue_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseEnumMember | EnumFormats.UseNumberValue);

		// Act
		var result1 = sut.ForElement().Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = sut.ForElement().Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.ForElement().Serialize(TestValue.MemberWithExplicitValue, _contextMock.Object);
		var result4 = sut.ForElement().Serialize(TestValue.MemberWithEnumMember, _contextMock.Object);
		var result5 = sut.ForElement().Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.GetTextValue().Should().BeEquivalentTo(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture));
		result2!.GetTextValue().Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result3!.GetTextValue().Should().BeEquivalentTo(MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture));
		result4!.GetTextValue().Should().BeEquivalentTo(MemberWithEnumMemberDataValue);
		result5!.GetTextValue().Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueMember);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_Element_FormatUseDescriptionOrUseNumberValue_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseDescription | EnumFormats.UseNumberValue);

		// Act
		var result1 = sut.ForElement().Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = sut.ForElement().Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.ForElement().Serialize(TestValue.MemberWithExplicitValue, _contextMock.Object);
		var result4 = sut.ForElement().Serialize(TestValue.MemberWithEnumMember, _contextMock.Object);
		var result5 = sut.ForElement().Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.GetTextValue().Should().BeEquivalentTo(MemberWithDescriptionDataValue);
		result2!.GetTextValue().Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result3!.GetTextValue().Should().BeEquivalentTo(MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture));
		result4!.GetTextValue().Should().BeEquivalentTo(MemberWithEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result5!.GetTextValue().Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueDescription);
	}
}
