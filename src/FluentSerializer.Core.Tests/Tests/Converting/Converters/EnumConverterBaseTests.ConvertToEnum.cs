using FluentAssertions;
using FluentSerializer.Core.Converting.Converters;
using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Converting.Converters;

public sealed partial class EnumConverterBaseTests
{
	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToEnum_ValueNull_ReturnsNull()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseDescription);

		// Act
		var result = sut.ConvertToEnum(null, typeof(TestEnum));

		// Assert
		result.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToEnum_FormatUseDescription_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseDescription);

		// Act
		var result1 = sut.ConvertToEnum(MemberWithDescriptionDescription, typeof(TestEnum));
		var result2 = sut.ConvertToEnum(MemberWithDescriptionValue.ToString(), typeof(TestEnum));

		// Assert
		result1.Should().BeEquivalentTo(TestEnum.MemberWithDescription);
		result2.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToEnum_FormatUseName_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseName);

		// Act
		var result1 = sut.ConvertToEnum(MemberWithDescriptionName, typeof(TestEnum));
		var result2 = sut.ConvertToEnum(MemberWithDescriptionDescription, typeof(TestEnum));
		var result3 = sut.ConvertToEnum(MemberWithDescriptionValue.ToString(), typeof(TestEnum));

		// Assert
		result1.Should().BeEquivalentTo(TestEnum.MemberWithDescription);
		result2.Should().BeNull();
		result3.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToEnum_FormatUseNumberValue_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseNumberValue);
		
		// Act
		var result1 = sut.ConvertToEnum(MemberWithoutDescriptionValue.ToString(), typeof(TestEnum));
		var result2 = sut.ConvertToEnum(MemberWithDescriptionDescription, typeof(TestEnum));
		var result3 = sut.ConvertToEnum(MemberWithExplicitValueValue.ToString(), typeof(TestEnum));

		// Assert
		result1.Should().BeEquivalentTo(TestEnum.MemberWithoutDescription);
		result2.Should().BeNull();
		result3.Should().BeEquivalentTo(TestEnum.MemberWithExplicitValue);
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToEnum_FormatUseDescriptionOrUseName_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseDescription | EnumFormat.UseName);

		// Act
		var result1 = sut.ConvertToEnum(MemberWithDescriptionDescription, typeof(TestEnum));
		var result2 = sut.ConvertToEnum(MemberWithDescriptionName, typeof(TestEnum));
		var result3 = sut.ConvertToEnum(MemberWithDescriptionValue.ToString(), typeof(TestEnum));

		// Assert
		result1.Should().BeEquivalentTo(TestEnum.MemberWithDescription);
		result2.Should().BeEquivalentTo(TestEnum.MemberWithDescription);
		result3.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToEnum_FormatUseDescriptionOrUseNumberValue_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseDescription | EnumFormat.UseNumberValue);

		// Act
		var result1 = sut.ConvertToEnum(MemberWithDescriptionDescription, typeof(TestEnum));
		var result2 = sut.ConvertToEnum(MemberWithDescriptionName, typeof(TestEnum));
		var result3 = sut.ConvertToEnum(MemberWithExplicitValueValue.ToString(), typeof(TestEnum));

		// Assert
		result1.Should().BeEquivalentTo(TestEnum.MemberWithDescription);
		result2.Should().BeNull();
		result3.Should().BeEquivalentTo(TestEnum.MemberWithExplicitValue);
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToEnum_FormatUseNameOrUseNumberValue_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseName | EnumFormat.UseNumberValue);

		// Act
		var result1 = sut.ConvertToEnum(MemberWithDescriptionDescription, typeof(TestEnum));
		var result2 = sut.ConvertToEnum(MemberWithDescriptionName, typeof(TestEnum));
		var result3 = sut.ConvertToEnum(MemberWithExplicitValueValue.ToString(), typeof(TestEnum));
		var result4 = sut.ConvertToEnum(MemberWithExplicitValueName, typeof(TestEnum));

		// Assert
		result1.Should().BeNull();
		result2.Should().BeEquivalentTo(TestEnum.MemberWithDescription);
		result3.Should().BeEquivalentTo(TestEnum.MemberWithExplicitValue);
		result4.Should().BeEquivalentTo(TestEnum.MemberWithExplicitValue);
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToEnum_FormatUseAll_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.UseDescription | EnumFormat.UseName | EnumFormat.UseNumberValue);

		// Act
		var result1 = sut.ConvertToEnum(MemberWithDescriptionDescription, typeof(TestEnum));
		var result2 = sut.ConvertToEnum(MemberWithDescriptionName, typeof(TestEnum));
		var result3 = sut.ConvertToEnum(MemberWithExplicitValueValue.ToString(), typeof(TestEnum));

		// Assert
		result1.Should().BeEquivalentTo(TestEnum.MemberWithDescription);
		result2.Should().BeEquivalentTo(TestEnum.MemberWithDescription);
		result3.Should().BeEquivalentTo(TestEnum.MemberWithExplicitValue);
	}
}
