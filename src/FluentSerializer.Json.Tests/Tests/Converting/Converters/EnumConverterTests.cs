using FluentAssertions;

using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Core.Naming;
using FluentSerializer.Core.Tests.ObjectMother;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Services;

using Moq;

using System;
using System.ComponentModel;
using System.Runtime.Serialization;

using Xunit;

using EnumConverter = FluentSerializer.Json.Converting.Converters.EnumConverter;

namespace FluentSerializer.Json.Tests.Tests.Converting.Converters;

public sealed partial class EnumConverterTests
{
	private readonly Mock<ISerializerContext<IJsonNode>> _contextMock;
	public EnumConverterTests()
	{
		var serializerMock = new Mock<IAdvancedJsonSerializer>();
		_contextMock = new Mock<ISerializerContext<IJsonNode>>()
			.SetupDefault(serializerMock)
			.WithNamingStrategy(Names.Use.CamelCase)
			.WithPropertyType(typeof(TestValue));
	}

	[Theory,
		InlineData(typeof(EnumFormats)), InlineData(typeof(TestValue)),
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void CanConvert_IsEnum_ReturnsTrue(Type input)
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.Default, false);

		// Act
		var result = sut.CanConvert(in input);

		// Assert
		result.Should().BeTrue();
	}

	[Theory,
		InlineData(typeof(bool)), InlineData(typeof(int)), InlineData(typeof(string)),
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void CanConvert_IsNotEnum_ReturnsFalse(Type input)
	{
		// Arrange
		var sut = new EnumConverter(EnumFormats.Default, false);

		// Act
		var result = sut.CanConvert(in input);

		// Assert
		result.Should().BeFalse();
	}

	private const string MemberWithoutDescriptionOrEnumMemberName = nameof(TestValue.MemberWithoutDescriptionOrEnumMember);
	private const long MemberWithoutDescriptionOrEnumMemberValue = (int)TestValue.MemberWithoutDescriptionOrEnumMember;
	private const string MemberWithEnumMemberName = nameof(TestValue.MemberWithEnumMember);
	private const long MemberWithEnumMemberValue = (int)TestValue.MemberWithEnumMember;
	private const string MemberWithEnumMemberDataValue = "MemberValue";
	private const string MemberWithDescriptionName = nameof(TestValue.MemberWithDescription);
	private const long MemberWithDescriptionValue = (int)TestValue.MemberWithDescription;
	private const string MemberWithDescriptionDataValue = "This member has a description";
	private const string MemberWithEnumMemberAndDescriptionName = nameof(TestValue.MemberWithEnumMemberAndDescription);
	private const long MemberWithEnumMemberAndDescriptionValue = (int)TestValue.MemberWithEnumMemberAndDescription;
	private const string MemberWithEnumMemberAndDescriptionDataValueMember = "MemberValueWithDescription";
	private const string MemberWithEnumMemberAndDescriptionDataValueDescription = "This member has a description and an enum member attribute";
	private const string MemberWithExplicitValueName = nameof(TestValue.MemberWithExplicitValue);
	private const long MemberWithExplicitValueValue = 9239202348;

	private enum TestValue : long
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
}
