using FluentAssertions;

using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;

using System;

using Xunit;

namespace FluentSerializer.Json.Tests.Tests;

/// <summary>
/// Testing all nodes combined for the <see cref="JsonBuilder"/>
/// </summary>
public sealed class JsonBuilderTests
{
	[Theory,
		Trait("Category", "IntegrationTest"),	Trait("DataFormat", "JSON"),
		InlineData(true), InlineData(false)]
	public void AllObjects_EqualsExpectedTextData(bool format)
	{
		// Arrange
		var expected = AllJsonNodes.GetJson(format);
		var input = AllJsonNodes.GetInstance(format);

		// Act
		var result = input.WriteTo(TestStringBuilderPool.Default, format);

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "IntegrationTest"), Trait("DataFormat", "JSON")]
	public void Property_NullName_Throws()
	{
		// Act
		var result1 = () => JsonBuilder.Property(null!, JsonBuilder.Value(null));
		var result2 = () => JsonBuilder.Property(null!, JsonBuilder.Array());
		var result3 = () => JsonBuilder.Property(null!, JsonBuilder.Object());

		// Assert
		result1
			.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName("name")
			.And
				.StackTrace!.Split(Environment.NewLine).Length
				.Should().Be(8);
		result2
			.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName("name")
			.And
				.StackTrace!.Split(Environment.NewLine).Length
				.Should().Be(8);
		result3
			.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName("name")
			.And
				.StackTrace!.Split(Environment.NewLine).Length
				.Should().Be(8);
	}

	[Fact,
		Trait("Category", "IntegrationTest"), Trait("DataFormat", "JSON")]
	public void Property_InvalidName_Throws()
	{
		// Act
		var invalidText = "Some: invalid <text>";

		var result1 = () => JsonBuilder.Property(invalidText, JsonBuilder.Value(null));
		var result2 = () => JsonBuilder.Property(invalidText, JsonBuilder.Array());
		var result3 = () => JsonBuilder.Property(invalidText, JsonBuilder.Object());

		// Assert
		result1
			.Should()
			.ThrowExactly<ArgumentException>()
			.WithParameterName("name")
			.And
				.StackTrace!.Split(Environment.NewLine).Length
				.Should().Be(6);
		result2
			.Should()
			.ThrowExactly<ArgumentException>()
			.WithParameterName("name")
			.And
				.StackTrace!.Split(Environment.NewLine).Length
				.Should().Be(6);
		result3
			.Should()
			.ThrowExactly<ArgumentException>()
			.WithParameterName("name")
			.And
				.StackTrace!.Split(Environment.NewLine).Length
				.Should().Be(6);
	}

	[Fact,
		Trait("Category", "IntegrationTest"), Trait("DataFormat", "JSON")]
	public void Comment_NullValue_Throws()
	{
		// Act
		var result = () => JsonBuilder.Comment(null!);

		// Assert
		result
			.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName("value")
			.And
				.StackTrace!.Split(Environment.NewLine).Length
				.Should().Be(6);
	}

	[Fact,
		Trait("Category", "IntegrationTest"), Trait("DataFormat", "JSON")]
	public void MultilineComment_NullValue_Throws()
	{
		// Act
		var result = () => JsonBuilder.MultilineComment(null!);

		// Assert
		result
			.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName("value")
			.And
				.StackTrace!.Split(Environment.NewLine).Length
				.Should().Be(6);
	}

	[Fact,
		Trait("Category", "IntegrationTest"), Trait("DataFormat", "JSON")]
	public void Comment_InvalidValue_Throws()
	{
		// Act
		var invalidText = "Some text with a \r\nnewline";

		var result4 = () => JsonBuilder.Comment(invalidText);

		// Assert
		result4
			.Should()
			.ThrowExactly<ArgumentException>()
			.WithMessage("A single line comment cannot contain newline characters *")
			.WithParameterName("value")
			.And
				.StackTrace!.Split(Environment.NewLine).Length
				.Should().Be(5);
	}
}