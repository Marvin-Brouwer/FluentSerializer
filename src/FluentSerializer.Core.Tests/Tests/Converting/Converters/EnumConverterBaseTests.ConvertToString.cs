using System;
using FluentAssertions;
using FluentSerializer.Core.Converting.Converters;
using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Converting.Converters;

public sealed partial class EnumConverterBaseTests
{
	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToString_FormatUseEnumMember_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseEnumMember);

		// Act
		var result1 = () => sut.ConvertToString(TestEnum.MemberWithDescription, typeof(TestEnum));
		var result2 = () => sut.ConvertToString(TestEnum.MemberWithoutDescriptionOrEnumMember, typeof(TestEnum));
		var result3 = sut.ConvertToString(TestEnum.MemberWithEnumMember, typeof(TestEnum));
		var result4 = sut.ConvertToString(TestEnum.MemberWithEnumMemberAndDescription, typeof(TestEnum));

		// Assert
		result1.Should().ThrowExactly<NotSupportedException>();
		result2.Should().ThrowExactly<NotSupportedException>();
		result3!.Value.value.Should().BeEquivalentTo(MemberWithEnumMemberDataValue);
		result3!.Value.isNumeric.Should().BeFalse();
		result4!.Value.value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueMember);
		result4!.Value.isNumeric.Should().BeFalse();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToString_FormatUseDescription_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseDescription);

		// Act
		var result1 = sut.ConvertToString(TestEnum.MemberWithDescription, typeof(TestEnum));
		var result2 = () => sut.ConvertToString(TestEnum.MemberWithoutDescriptionOrEnumMember, typeof(TestEnum));
		var result3 = () => sut.ConvertToString(TestEnum.MemberWithEnumMember, typeof(TestEnum));
		var result4 = sut.ConvertToString(TestEnum.MemberWithEnumMemberAndDescription, typeof(TestEnum));

		// Assert
		result1!.Value.value.Should().BeEquivalentTo(MemberWithDescriptionDataValue);
		result1!.Value.isNumeric.Should().BeFalse();
		result2.Should().ThrowExactly<NotSupportedException>();
		result3.Should().ThrowExactly<NotSupportedException>();
		result4!.Value.value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueDescription);
		result4!.Value.isNumeric.Should().BeFalse();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToString_FormatUseName_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseName);

		// Act
		var result1 = sut.ConvertToString(TestEnum.MemberWithDescription, typeof(TestEnum));
		var result2 = sut.ConvertToString(TestEnum.MemberWithoutDescriptionOrEnumMember, typeof(TestEnum));
		var result3 = sut.ConvertToString(TestEnum.MemberWithEnumMemberAndDescription, typeof(TestEnum));

		// Assert
		result1!.Value.value.Should().BeEquivalentTo(MemberWithDescriptionName);
		result1!.Value.isNumeric.Should().BeFalse();
		result2!.Value.value.Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberName);
		result2!.Value.isNumeric.Should().BeFalse();
		result3!.Value.value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionName);
		result3!.Value.isNumeric.Should().BeFalse();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToString_FormatUseNumberValue_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseNumberValue);

		// Act
		var result1 = sut.ConvertToString(TestEnum.MemberWithDescription, typeof(TestEnum));
		var result2 = sut.ConvertToString(TestEnum.MemberWithoutDescriptionOrEnumMember, typeof(TestEnum));
		var result3 = sut.ConvertToString(TestEnum.MemberWithExplicitValue, typeof(TestEnum));
		var result4 = sut.ConvertToString(TestEnum.MemberWithEnumMemberAndDescription, typeof(TestEnum));

		// Assert
		result1!.Value.value.Should().BeEquivalentTo(MemberWithDescriptionValue.ToString());
		result1!.Value.isNumeric.Should().BeTrue();
		result2!.Value.value.Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString());
		result2!.Value.isNumeric.Should().BeTrue();
		result3!.Value.value.Should().BeEquivalentTo(MemberWithExplicitValueValue.ToString());
		result3!.Value.isNumeric.Should().BeTrue();
		result4!.Value.value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionValue.ToString());
		result4!.Value.isNumeric.Should().BeTrue();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToString_FormatUseEnumMemberOrUseName_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseEnumMember | EnumFormat.UseName);

		// Act
		var result1 = sut.ConvertToString(TestEnum.MemberWithDescription, typeof(TestEnum));
		var result2 = sut.ConvertToString(TestEnum.MemberWithoutDescriptionOrEnumMember, typeof(TestEnum));
		var result3 = sut.ConvertToString(TestEnum.MemberWithEnumMember, typeof(TestEnum));
		var result4 = sut.ConvertToString(TestEnum.MemberWithEnumMemberAndDescription, typeof(TestEnum));

		// Assert
		result1!.Value.value.Should().BeEquivalentTo(MemberWithDescriptionName);
		result1!.Value.isNumeric.Should().BeFalse();
		result2!.Value.value.Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberName);
		result2!.Value.isNumeric.Should().BeFalse();
		result3!.Value.value.Should().BeEquivalentTo(MemberWithEnumMemberDataValue);
		result3!.Value.isNumeric.Should().BeFalse();
		result4!.Value.value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueMember);
		result4!.Value.isNumeric.Should().BeFalse();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToString_FormatUseDescriptionOrUseName_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseDescription | EnumFormat.UseName);

		// Act
		var result1 = sut.ConvertToString(TestEnum.MemberWithDescription, typeof(TestEnum));
		var result2 = sut.ConvertToString(TestEnum.MemberWithoutDescriptionOrEnumMember, typeof(TestEnum));
		var result3 = sut.ConvertToString(TestEnum.MemberWithEnumMember, typeof(TestEnum));
		var result4 = sut.ConvertToString(TestEnum.MemberWithEnumMemberAndDescription, typeof(TestEnum));

		// Assert
		result1!.Value.value.Should().BeEquivalentTo(MemberWithDescriptionDataValue);
		result1!.Value.isNumeric.Should().BeFalse();
		result2!.Value.value.Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberName);
		result2!.Value.isNumeric.Should().BeFalse();
		result3!.Value.value.Should().BeEquivalentTo(MemberWithEnumMemberName);
		result3!.Value.isNumeric.Should().BeFalse();
		result4!.Value.value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueDescription);
		result4!.Value.isNumeric.Should().BeFalse();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToString_FormatUseEnumMemberOrUseNumberValue_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseEnumMember | EnumFormat.UseNumberValue);

		// Act
		var result1 = sut.ConvertToString(TestEnum.MemberWithDescription, typeof(TestEnum));
		var result2 = sut.ConvertToString(TestEnum.MemberWithoutDescriptionOrEnumMember, typeof(TestEnum));
		var result3 = sut.ConvertToString(TestEnum.MemberWithExplicitValue, typeof(TestEnum));
		var result4 = sut.ConvertToString(TestEnum.MemberWithEnumMember, typeof(TestEnum));
		var result5 = sut.ConvertToString(TestEnum.MemberWithEnumMemberAndDescription, typeof(TestEnum));

		// Assert
		result1!.Value.value.Should().BeEquivalentTo(MemberWithDescriptionValue.ToString());
		result1!.Value.isNumeric.Should().BeTrue();
		result2!.Value.value.Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString());
		result2!.Value.isNumeric.Should().BeTrue();
		result3!.Value.value.Should().BeEquivalentTo(MemberWithExplicitValueValue.ToString());
		result3!.Value.isNumeric.Should().BeTrue();
		result4!.Value.value.Should().BeEquivalentTo(MemberWithEnumMemberDataValue);
		result4!.Value.isNumeric.Should().BeFalse();
		result5!.Value.value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueMember);
		result5!.Value.isNumeric.Should().BeFalse();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToString_FormatUseDescriptionOrUseNumberValue_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseDescription | EnumFormat.UseNumberValue);

		// Act
		var result1 = sut.ConvertToString(TestEnum.MemberWithDescription, typeof(TestEnum));
		var result2 = sut.ConvertToString(TestEnum.MemberWithoutDescriptionOrEnumMember, typeof(TestEnum));
		var result3 = sut.ConvertToString(TestEnum.MemberWithExplicitValue, typeof(TestEnum));
		var result4 = sut.ConvertToString(TestEnum.MemberWithEnumMember, typeof(TestEnum));
		var result5 = sut.ConvertToString(TestEnum.MemberWithEnumMemberAndDescription, typeof(TestEnum));

		// Assert
		result1!.Value.value.Should().BeEquivalentTo(MemberWithDescriptionDataValue);
		result1!.Value.isNumeric.Should().BeFalse();
		result2!.Value.value.Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberValue.ToString());
		result2!.Value.isNumeric.Should().BeTrue();
		result3!.Value.value.Should().BeEquivalentTo(MemberWithExplicitValueValue.ToString());
		result3!.Value.isNumeric.Should().BeTrue();
		result4!.Value.value.Should().BeEquivalentTo(MemberWithEnumMemberValue.ToString());
		result4!.Value.isNumeric.Should().BeTrue();
		result5!.Value.value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueDescription);
		result5!.Value.isNumeric.Should().BeFalse();
	}

	// The next two tests don't mean anything but they're included for completeness

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToString_FormatUseNameOrUseNumberValue_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseName | EnumFormat.UseNumberValue);

		// Act
		var result1 = sut.ConvertToString(TestEnum.MemberWithDescription, typeof(TestEnum));
		var result2 = sut.ConvertToString(TestEnum.MemberWithoutDescriptionOrEnumMember, typeof(TestEnum));

		// Assert
		result1!.Value.value.Should().BeEquivalentTo(MemberWithDescriptionName);
		result1!.Value.isNumeric.Should().BeFalse();
		result2!.Value.value.Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberName);
		result2!.Value.isNumeric.Should().BeFalse();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToString_FormatUseAll_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseEnumMember | EnumFormat.UseDescription | EnumFormat.UseName | EnumFormat.UseNumberValue);

		// Act
		var result1 = sut.ConvertToString(TestEnum.MemberWithDescription, typeof(TestEnum));
		var result2 = sut.ConvertToString(TestEnum.MemberWithoutDescriptionOrEnumMember, typeof(TestEnum));
		var result3 = sut.ConvertToString(TestEnum.MemberWithEnumMember, typeof(TestEnum));
		var result4 = sut.ConvertToString(TestEnum.MemberWithEnumMemberAndDescription, typeof(TestEnum));

		// Assert
		result1!.Value.value.Should().BeEquivalentTo(MemberWithDescriptionDataValue);
		result1!.Value.isNumeric.Should().BeFalse();
		result2!.Value.value.Should().BeEquivalentTo(MemberWithoutDescriptionOrEnumMemberName);
		result2!.Value.isNumeric.Should().BeFalse();
		result3!.Value.value.Should().BeEquivalentTo(MemberWithEnumMemberDataValue);
		result3!.Value.isNumeric.Should().BeFalse();
		result4!.Value.value.Should().BeEquivalentTo(MemberWithEnumMemberAndDescriptionDataValueMember);
		result4!.Value.isNumeric.Should().BeFalse();
	}
}
