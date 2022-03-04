using System;
using FluentAssertions;
using FluentSerializer.Core.Converting.Converters;
using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Converting.Converters;

public sealed partial class EnumConverterBaseTests
{
	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToString_FormatUseDescription_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseDescription);

		// Act
		var result1 = sut.ConvertToString(TestEnum.MemberWithDescription, typeof(TestEnum));
		var result2 = () => sut.ConvertToString(TestEnum.MemberWithoutDescription, typeof(TestEnum));

		// Assert
		result1!.Value.value.Should().BeEquivalentTo(MemberWithDescriptionDescription);
		result1!.Value.isNumeric.Should().BeFalse();
		result2.Should().ThrowExactly<NotSupportedException>();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToString_FormatUseName_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseName);

		// Act
		var result1 = sut.ConvertToString(TestEnum.MemberWithDescription, typeof(TestEnum));
		var result2 = sut.ConvertToString(TestEnum.MemberWithoutDescription, typeof(TestEnum));

		// Assert
		result1!.Value.value.Should().BeEquivalentTo(MemberWithDescriptionName);
		result1!.Value.isNumeric.Should().BeFalse();
		result2!.Value.value.Should().BeEquivalentTo(MemberWithoutDescriptionName);
		result2!.Value.isNumeric.Should().BeFalse();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToString_FormatUseNumberValue_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseNumberValue);

		// Act
		var result1 = sut.ConvertToString(TestEnum.MemberWithDescription, typeof(TestEnum));
		var result2 = sut.ConvertToString(TestEnum.MemberWithoutDescription, typeof(TestEnum));
		var result3 = sut.ConvertToString(TestEnum.MemberWithExplicitValue, typeof(TestEnum));

		// Assert
		result1!.Value.value.Should().BeEquivalentTo(MemberWithDescriptionValue.ToString());
		result1!.Value.isNumeric.Should().BeTrue();
		result2!.Value.value.Should().BeEquivalentTo(MemberWithoutDescriptionValue.ToString());
		result2!.Value.isNumeric.Should().BeTrue();
		result3!.Value.value.Should().BeEquivalentTo(MemberWithExplicitValueValue.ToString());
		result3!.Value.isNumeric.Should().BeTrue();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToString_FormatUseDescriptionOrUseName_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseDescription | EnumFormat.UseName);

		// Act
		var result1 = sut.ConvertToString(TestEnum.MemberWithDescription, typeof(TestEnum));
		var result2 = sut.ConvertToString(TestEnum.MemberWithoutDescription, typeof(TestEnum));

		// Assert
		result1!.Value.value.Should().BeEquivalentTo(MemberWithDescriptionDescription);
		result1!.Value.isNumeric.Should().BeFalse();
		result2!.Value.value.Should().BeEquivalentTo(MemberWithoutDescriptionName);
		result2!.Value.isNumeric.Should().BeFalse();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToString_FormatUseDescriptionOrUseNumberValue_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseDescription | EnumFormat.UseNumberValue);

		// Act
		var result1 = sut.ConvertToString(TestEnum.MemberWithDescription, typeof(TestEnum));
		var result2 = sut.ConvertToString(TestEnum.MemberWithoutDescription, typeof(TestEnum));
		var result3 = sut.ConvertToString(TestEnum.MemberWithExplicitValue, typeof(TestEnum));

		// Assert
		result1!.Value.value.Should().BeEquivalentTo(MemberWithDescriptionDescription);
		result1!.Value.isNumeric.Should().BeFalse();
		result2!.Value.value.Should().BeEquivalentTo(MemberWithoutDescriptionValue.ToString());
		result2!.Value.isNumeric.Should().BeTrue();
		result3!.Value.value.Should().BeEquivalentTo(MemberWithExplicitValueValue.ToString());
		result3!.Value.isNumeric.Should().BeTrue();
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
		var result2 = sut.ConvertToString(TestEnum.MemberWithoutDescription, typeof(TestEnum));

		// Assert
		result1!.Value.value.Should().BeEquivalentTo(MemberWithDescriptionName);
		result1!.Value.isNumeric.Should().BeFalse();
		result2!.Value.value.Should().BeEquivalentTo(MemberWithoutDescriptionName);
		result2!.Value.isNumeric.Should().BeFalse();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToString_FormatUseAll_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseDescription | EnumFormat.UseName | EnumFormat.UseNumberValue);

		// Act
		var result1 = sut.ConvertToString(TestEnum.MemberWithDescription, typeof(TestEnum));
		var result2 = sut.ConvertToString(TestEnum.MemberWithoutDescription, typeof(TestEnum));

		// Assert
		result1!.Value.value.Should().BeEquivalentTo(MemberWithDescriptionDescription);
		result1!.Value.isNumeric.Should().BeFalse();
		result2!.Value.value.Should().BeEquivalentTo(MemberWithoutDescriptionName);
		result2!.Value.isNumeric.Should().BeFalse();
	}
}
