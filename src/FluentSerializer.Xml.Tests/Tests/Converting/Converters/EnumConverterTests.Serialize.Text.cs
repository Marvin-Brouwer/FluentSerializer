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
	public void Serialize_Text_FormatUseEnumMember_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseEnumMember);

		// Act
		var result1 = () => sut.ForText().Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = () => sut.ForText().Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.ForText().Serialize(TestValue.MemberWithEnumMember, _contextMock.Object);
		var result4 = sut.ForText().Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1.Should().ThrowExactly<NotSupportedException>();
		result2.Should().ThrowExactly<NotSupportedException>();
		result3!.Value.Should().BeEquivalentTo(MemberWithEnumMemberDataValue);
		result3!.Value.Should().NotBeEquivalentTo(MemberWithEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result4!.Value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueMember);
		result4!.Value.Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionValue.ToString(CultureInfo.InvariantCulture));
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_Text_FormatUseDescription_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseDescription);

		// Act
		var result1 = sut.ForText().Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = () => sut.ForText().Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = () => sut.ForText().Serialize(TestValue.MemberWithEnumMember, _contextMock.Object);
		var result4 = sut.ForText().Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.Value.Should().BeEquivalentTo(MemberWithDescriptionDataValue);
		result1!.Value.Should().NotBeEquivalentTo(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture));
		result2.Should().ThrowExactly<NotSupportedException>();
		result3.Should().ThrowExactly<NotSupportedException>();
		result4!.Value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueDescription);
		result4!.Value.Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionValue.ToString(CultureInfo.InvariantCulture));
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_Text_FormatUseName_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseName);

		// Act
		var result1 = sut.ForText().Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = sut.ForText().Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.ForText().Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.Value.Should().BeEquivalentTo(MemberWithDescriptionName);
		result1!.Value.Should().NotBeEquivalentTo(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture));
		result2!.Value.Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberName);
		result2!.Value.Should().NotBeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result3!.Value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionName);
		result3!.Value.Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionValue.ToString(CultureInfo.InvariantCulture));
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_Text_FormatUseNumberValue_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseNumberValue);

		// Act
		var result1 = sut.ForText().Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = sut.ForText().Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.ForText().Serialize(TestValue.MemberWithExplicitValue, _contextMock.Object);
		var result4 = sut.ForText().Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.Value.Should().BeEquivalentTo(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture));
		result1!.Value.Should().NotBeEquivalentTo(MemberWithDescriptionName);
		result1!.Value.Should().NotBeEquivalentTo(MemberWithEnumMemberDataValue);
		result2!.Value.Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result2!.Value.Should().NotBeEquivalentTo(MemberWithDescriptionName);
		result3!.Value.Should().BeEquivalentTo(MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture));
		result3!.Value.Should().NotBeEquivalentTo(MemberWithExplicitValueName);
		result4!.Value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionValue.ToString(CultureInfo.InvariantCulture));
		result4!.Value.Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionName);
		result4!.Value.Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueMember);
		result4!.Value.Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueDescription);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_Text_FormatUseEnumMemberOrUseName_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseEnumMember | EnumFormats.UseName);

		// Act
		var result1 = sut.ForText().Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = sut.ForText().Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.ForText().Serialize(TestValue.MemberWithEnumMember, _contextMock.Object);
		var result4 = sut.ForText().Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.Value.Should().BeEquivalentTo(MemberWithDescriptionName);
		result1!.Value.Should().NotBeEquivalentTo(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture));
		result2!.Value.Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberName);
		result2!.Value.Should().NotBeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result3!.Value.Should().BeEquivalentTo(MemberWithEnumMemberDataValue);
		result3!.Value.Should().NotBeEquivalentTo(MemberWithEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result4!.Value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueMember);
		result4!.Value.Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionValue.ToString(CultureInfo.InvariantCulture));
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_Text_FormatUseDescriptionOrUseName_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseDescription | EnumFormats.UseName);

		// Act
		var result1 = sut.ForText().Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = sut.ForText().Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.ForText().Serialize(TestValue.MemberWithEnumMember, _contextMock.Object);
		var result4 = sut.ForText().Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.Value.Should().BeEquivalentTo(MemberWithDescriptionDataValue);
		result1!.Value.Should().NotBeEquivalentTo(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture));
		result2!.Value.Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberName);
		result2!.Value.Should().NotBeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result3!.Value.Should().BeEquivalentTo(MemberWithEnumMemberName);
		result3!.Value.Should().NotBeEquivalentTo(MemberWithEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result4!.Value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueDescription);
		result4!.Value.Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionValue.ToString(CultureInfo.InvariantCulture));
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_Text_FormatUseEnumMemberOrUseNumberValue_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseEnumMember | EnumFormats.UseNumberValue);

		// Act
		var result1 = sut.ForText().Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = sut.ForText().Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.ForText().Serialize(TestValue.MemberWithExplicitValue, _contextMock.Object);
		var result4 = sut.ForText().Serialize(TestValue.MemberWithEnumMember, _contextMock.Object);
		var result5 = sut.ForText().Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.Value.Should().BeEquivalentTo(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture));
		result2!.Value.Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result3!.Value.Should().BeEquivalentTo(MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture));
		result4!.Value.Should().BeEquivalentTo(MemberWithEnumMemberDataValue);
		result5!.Value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueMember);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_Text_FormatUseDescriptionOrUseNumberValue_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseDescription | EnumFormats.UseNumberValue);

		// Act
		var result1 = sut.ForText().Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = sut.ForText().Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.ForText().Serialize(TestValue.MemberWithExplicitValue, _contextMock.Object);
		var result4 = sut.ForText().Serialize(TestValue.MemberWithEnumMember, _contextMock.Object);
		var result5 = sut.ForText().Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.Value.Should().BeEquivalentTo(MemberWithDescriptionDataValue);
		result2!.Value.Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result3!.Value.Should().BeEquivalentTo(MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture));
		result4!.Value.Should().BeEquivalentTo(MemberWithEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result5!.Value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueDescription);
	}
}
