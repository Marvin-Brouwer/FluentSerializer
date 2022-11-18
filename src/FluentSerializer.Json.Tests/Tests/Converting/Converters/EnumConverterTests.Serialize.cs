using FluentAssertions;

using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Json.Converter.DefaultJson.Extensions;
using FluentSerializer.Json.Converting.Converters;
using FluentSerializer.Json.DataNodes;

using System;
using System.Globalization;

using Xunit;

namespace FluentSerializer.Json.Tests.Tests.Converting.Converters;

public sealed partial class EnumConverterTests
{
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_FormatUseEnumMember_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseEnumMember, false);

		// Act
		var result1 = () => sut.Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = () => sut.Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.Serialize(TestValue.MemberWithEnumMember, _contextMock.Object);
		var result4 = sut.Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1.Should().ThrowExactly<NotSupportedException>();
		result2.Should().ThrowExactly<NotSupportedException>();
		result3!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithEnumMemberDataValue.WrapString());
		result3!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result4!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueMember.WrapString());
		result4!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionValue.ToString(CultureInfo.InvariantCulture));
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_FormatUseDescription_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseDescription, false);

		// Act
		var result1 = sut.Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = () => sut.Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = () => sut.Serialize(TestValue.MemberWithEnumMember, _contextMock.Object);
		var result4 = sut.Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithDescriptionDataValue.WrapString());
		result1!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture));
		result2.Should().ThrowExactly<NotSupportedException>();
		result3.Should().ThrowExactly<NotSupportedException>();
		result4!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueDescription.WrapString());
		result4!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionValue.ToString(CultureInfo.InvariantCulture));
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_FormatUseName_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseName, false);

		// Act
		var result1 = sut.Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = sut.Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithDescriptionName.WrapString());
		result1!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture));
		result2!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberName.WrapString());
		result2!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result3!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionName.WrapString());
		result3!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionValue.ToString(CultureInfo.InvariantCulture));
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_FormatUseNumberValue_WriteAsNumber_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseNumberValue, false);

		// Act
		var result1 = sut.Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = sut.Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.Serialize(TestValue.MemberWithExplicitValue, _contextMock.Object);
		var result4 = sut.Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture));
		result1!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithDescriptionName);
		result1!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithEnumMemberDataValue);
		result2!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result2!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithDescriptionName);
		result3!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture));
		result3!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithExplicitValueName);
		result4!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionValue.ToString(CultureInfo.InvariantCulture));
		result4!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionName);
		result4!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueMember);
		result4!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueDescription);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_FormatUseNumberValue_WriteAsString_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseNumberValue, true);

		// Act
		var result1 = sut.Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = sut.Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.Serialize(TestValue.MemberWithExplicitValue, _contextMock.Object);
		var result4 = sut.Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture).WrapString());
		result2!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture).WrapString());
		result3!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture).WrapString());
		result4!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionValue.ToString(CultureInfo.InvariantCulture).WrapString());
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_FormatUseEnumMemberOrUseName_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseEnumMember | EnumFormats.UseName, false);

		// Act
		var result1 = sut.Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = sut.Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.Serialize(TestValue.MemberWithEnumMember, _contextMock.Object);
		var result4 = sut.Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithDescriptionName.WrapString());
		result1!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture));
		result2!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberName.WrapString());
		result2!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result3!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithEnumMemberDataValue.WrapString());
		result3!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result4!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueMember.WrapString());
		result4!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionValue.ToString(CultureInfo.InvariantCulture));
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_FormatUseDescriptionOrUseName_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseDescription | EnumFormats.UseName, false);

		// Act
		var result1 = sut.Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = sut.Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.Serialize(TestValue.MemberWithEnumMember, _contextMock.Object);
		var result4 = sut.Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithDescriptionDataValue.WrapString());
		result1!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture));
		result2!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberName.WrapString());
		result2!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result3!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithEnumMemberName.WrapString());
		result3!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result4!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueDescription.WrapString());
		result4!.As<IJsonValue>().Value.Should().NotBeEquivalentTo(MemberWithEnumMemberAndDescriptionValue.ToString(CultureInfo.InvariantCulture));
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_FormatUseEnumMemberOrUseNumberValue_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseEnumMember | EnumFormats.UseNumberValue, false);

		// Act
		var result1 = sut.Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = sut.Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.Serialize(TestValue.MemberWithExplicitValue, _contextMock.Object);
		var result4 = sut.Serialize(TestValue.MemberWithEnumMember, _contextMock.Object);
		var result5 = sut.Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture));
		result2!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result3!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture));
		result4!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithEnumMemberDataValue.WrapString());
		result5!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueMember.WrapString());
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Serialize_FormatUseDescriptionOrUseNumberValue_Returns()
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.UseDescription | EnumFormats.UseNumberValue, false);

		// Act
		var result1 = sut.Serialize(TestValue.MemberWithDescription, _contextMock.Object);
		var result2 = sut.Serialize(TestValue.MemberWithoutDescriptionOrEnumMember, _contextMock.Object);
		var result3 = sut.Serialize(TestValue.MemberWithExplicitValue, _contextMock.Object);
		var result4 = sut.Serialize(TestValue.MemberWithEnumMember, _contextMock.Object);
		var result5 = sut.Serialize(TestValue.MemberWithEnumMemberAndDescription, _contextMock.Object);

		// Assert
		result1!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithDescriptionDataValue.WrapString());
		result2!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result3!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture));
		result4!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithEnumMemberValue.ToString(CultureInfo.InvariantCulture));
		result5!.As<IJsonValue>().Value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueDescription.WrapString());
	}
}
