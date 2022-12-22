using FluentAssertions;

using FluentSerializer.Core.Converting.Converters;

using System.Globalization;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Converting.Converters;

public sealed partial class EnumConverterBaseTests
{
	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToEnum_ValueNull_ReturnsNull()
	{
		// Arrange
		var sut = new TestConverter(EnumFormats.UseDescription);

		// Act
		var result = sut.ConvertToEnum(null, typeof(TestValue));

		// Assert
		result.Should().BeNull();
	}

	[Fact,
	 Trait("Category", "UnitTest")]
	public void ConvertToEnum_FormatUseEnumMember_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormats.UseEnumMember);

		// Act
		var result1 = sut.ConvertToEnum(MemberWithEnumMemberDataValue, typeof(TestValue));
		var result2 = sut.ConvertToEnum(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture), typeof(TestValue));

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithEnumMember);
		result2.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToEnum_FormatUseDescription_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormats.UseDescription);

		// Act
		var result1 = sut.ConvertToEnum(MemberWithDescriptionDataValue, typeof(TestValue));
		var result2 = sut.ConvertToEnum(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture), typeof(TestValue));

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result2.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToEnum_FormatUseName_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormats.UseName);

		// Act
		var result1 = sut.ConvertToEnum(MemberWithDescriptionName, typeof(TestValue));
		var result2 = sut.ConvertToEnum(MemberWithDescriptionDataValue, typeof(TestValue));
		var result3 = sut.ConvertToEnum(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture), typeof(TestValue));

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result2.Should().BeNull();
		result3.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToEnum_FormatUseNumberValue_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormats.UseNumberValue);

		// Act
		var result1 = sut.ConvertToEnum(MemberWithoutDescriptionOrEnumMemberValue.ToString(CultureInfo.InvariantCulture), typeof(TestValue));
		var result2 = sut.ConvertToEnum(MemberWithDescriptionDataValue, typeof(TestValue));
		var result3 = sut.ConvertToEnum(MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture), typeof(TestValue));

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithoutDescriptionOrEnumMember);
		result2.Should().BeNull();
		result3.Should().BeEquivalentTo(TestValue.MemberWithExplicitValue);
	}

	[Fact,
	 Trait("Category", "UnitTest")]
	public void ConvertToEnum_FormatUseEnumMemberOrUseName_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormats.UseEnumMember | EnumFormats.UseName);

		// Act
		var result1 = sut.ConvertToEnum(MemberWithEnumMemberDataValue, typeof(TestValue));
		var result2 = sut.ConvertToEnum(MemberWithEnumMemberName, typeof(TestValue));
		var result3 = sut.ConvertToEnum(MemberWithEnumMemberValue.ToString(CultureInfo.InvariantCulture), typeof(TestValue));

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithEnumMember);
		result2.Should().BeEquivalentTo(TestValue.MemberWithEnumMember);
		result3.Should().BeNull();
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToEnum_FormatUseDescriptionOrUseName_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormats.UseDescription | EnumFormats.UseName);

		// Act
		var result1 = sut.ConvertToEnum(MemberWithDescriptionDataValue, typeof(TestValue));
		var result2 = sut.ConvertToEnum(MemberWithDescriptionName, typeof(TestValue));
		var result3 = sut.ConvertToEnum(MemberWithDescriptionValue.ToString(CultureInfo.InvariantCulture), typeof(TestValue));

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result2.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result3.Should().BeNull();
	}

	[Fact,
	 Trait("Category", "UnitTest")]
	public void ConvertToEnum_FormatUseEnumMemberOrUseNumberValue_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormats.UseEnumMember | EnumFormats.UseNumberValue);

		// Act
		var result1 = sut.ConvertToEnum(MemberWithEnumMemberDataValue, typeof(TestValue));
		var result2 = sut.ConvertToEnum(MemberWithEnumMemberName, typeof(TestValue));
		var result3 = sut.ConvertToEnum(MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture), typeof(TestValue));

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithEnumMember);
		result2.Should().BeNull();
		result3.Should().BeEquivalentTo(TestValue.MemberWithExplicitValue);
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToEnum_FormatUseDescriptionOrUseNumberValue_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormats.UseDescription | EnumFormats.UseNumberValue);

		// Act
		var result1 = sut.ConvertToEnum(MemberWithDescriptionDataValue, typeof(TestValue));
		var result2 = sut.ConvertToEnum(MemberWithDescriptionName, typeof(TestValue));
		var result3 = sut.ConvertToEnum(MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture), typeof(TestValue));

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result2.Should().BeNull();
		result3.Should().BeEquivalentTo(TestValue.MemberWithExplicitValue);
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToEnum_FormatUseNameOrUseNumberValue_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormats.UseName | EnumFormats.UseNumberValue);

		// Act
		var result1 = sut.ConvertToEnum(MemberWithDescriptionDataValue, typeof(TestValue));
		var result2 = sut.ConvertToEnum(MemberWithDescriptionName, typeof(TestValue));
		var result3 = sut.ConvertToEnum(MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture), typeof(TestValue));
		var result4 = sut.ConvertToEnum(MemberWithExplicitValueName, typeof(TestValue));

		// Assert
		result1.Should().BeNull();
		result2.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result3.Should().BeEquivalentTo(TestValue.MemberWithExplicitValue);
		result4.Should().BeEquivalentTo(TestValue.MemberWithExplicitValue);
	}

	[Fact,
		Trait("Category", "UnitTest")]
	public void ConvertToEnum_FormatUseAll_Returns()
	{
		// Arrange
		var sut = new TestConverter(EnumFormats.UseEnumMember | EnumFormats.UseDescription | EnumFormats.UseName | EnumFormats.UseNumberValue);

		// Act
		var result1 = sut.ConvertToEnum(MemberWithEnumMemberDataValue, typeof(TestValue));
		var result2 = sut.ConvertToEnum(MemberWithDescriptionDataValue, typeof(TestValue));
		var result3 = sut.ConvertToEnum(MemberWithDescriptionName, typeof(TestValue));
		var result4 = sut.ConvertToEnum(MemberWithExplicitValueValue.ToString(CultureInfo.InvariantCulture), typeof(TestValue));
		var result5 = sut.ConvertToEnum(MemberWithEnumMemberAndDescriptionDataValueMember, typeof(TestValue));

		// Assert
		result1.Should().BeEquivalentTo(TestValue.MemberWithEnumMember);
		result2.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result3.Should().BeEquivalentTo(TestValue.MemberWithDescription);
		result4.Should().BeEquivalentTo(TestValue.MemberWithExplicitValue);
		result5.Should().BeEquivalentTo(TestValue.MemberWithEnumMemberAndDescription);
	}
}
