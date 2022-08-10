using FluentAssertions;

using FluentSerializer.Core.Converting.Converters;

using System;
using System.ComponentModel;
using System.Runtime.Serialization;

using Xunit;

namespace FluentSerializer.Core.Tests.Tests.Converting.Converters;

public sealed partial class EnumConverterBaseTests
{
	[Theory,
		Trait("Category", "UnitTest"),
		InlineData(typeof(EnumFormat)), InlineData(typeof(TestEnum))]
	public void CanConvert_IsEnum_ReturnsTrue(Type input)
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.Default);

		// Act
		var result = sut.CanConvert(in input);

		// Assert
		result.Should().BeTrue();
	}

	[Theory,
		Trait("Category", "UnitTest"),
		InlineData(typeof(bool)), InlineData(typeof(int)), InlineData(typeof(string))]
	public void CanConvert_IsNotEnum_ReturnsFalse(Type input)
	{
		// Arrange
		var sut = new TestConverter(EnumFormat.Default);

		// Act
		var result = sut.CanConvert(in input);

		// Assert
		result.Should().BeFalse();
	}

	private const string MemberWithoutDescriptionOrEnumMemberName = nameof(TestEnum.MemberWithoutDescriptionOrEnumMember);
	private const long MemberWithoutDescriptionOrEnumMemberValue = (int)TestEnum.MemberWithoutDescriptionOrEnumMember;
	private const string MemberWithEnumMemberName = nameof(TestEnum.MemberWithEnumMember);
	private const long MemberWithEnumMemberValue = (int)TestEnum.MemberWithEnumMember;
	private const string MemberWithEnumMemberDataValue = "MemberValue";
	private const string MemberWithDescriptionName = nameof(TestEnum.MemberWithDescription);
	private const long MemberWithDescriptionValue = (int)TestEnum.MemberWithDescription;
	private const string MemberWithDescriptionDataValue = "This member has a description";
	private const string MemberWithEnumMemberAndDescriptionName = nameof(TestEnum.MemberWithEnumMemberAndDescription);
	private const long MemberWithEnumMemberAndDescriptionValue = (int)TestEnum.MemberWithEnumMemberAndDescription;
	private const string MemberWithEnumMemberAndDescriptionDataValueMember = "MemberValueWithDescription";
	private const string MemberWithEnumMemberAndDescriptionDataValueDescription = "This member has a description and an enum member attribute";
	private const string MemberWithExplicitValueName = nameof(TestEnum.MemberWithExplicitValue);
	private const long MemberWithExplicitValueValue = 9239202348;

	private enum TestEnum : long
	{
		MemberWithoutDescriptionOrEnumMember,
		[EnumMember(Value = MemberWithEnumMemberDataValue)]
		MemberWithEnumMember,
		[Description(MemberWithDescriptionDataValue)]
		MemberWithDescription,
		[EnumMember(Value = MemberWithEnumMemberAndDescriptionDataValueMember)]
		[Description(MemberWithEnumMemberAndDescriptionDataValueDescription)]
		MemberWithEnumMemberAndDescription,
		MemberWithExplicitValue = MemberWithExplicitValueValue
	}

	/// <inheritdoc />
	private class TestConverter : EnumConverterBase
	{
		/// <inheritdoc cref="EnumConverterBase.ConvertToString" />
		public new (string value, bool isNumeric)? ConvertToString(in object value, in Type enumType)
		{
			return base.ConvertToString(in value, in enumType);
		}

		/// <inheritdoc cref="EnumConverterBase.ConvertToEnum" />
		public new object? ConvertToEnum(in string? currentValue, in Type targetType)
		{
			return base.ConvertToEnum(in currentValue, in targetType);
		}

		public TestConverter(EnumFormat enumFormat) : base(enumFormat)
		{
		}
	}
}
