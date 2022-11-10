using FluentAssertions;

using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Xml.DataNodes;

using System;
using System.Collections.Generic;

using Xunit;

namespace FluentSerializer.Xml.Tests.Tests;

/// <summary>
/// Testing all nodes combined for the <see cref="XmlBuilder"/>
/// </summary>
public sealed class XmlBuilderTests
{
	[Theory,
		Trait("Category", "IntegrationTest"),	Trait("DataFormat", "XML"),
		InlineData(true), InlineData(false)]
	public void AllElements_EqualsExpectedTextData(bool format)
	{
		// Arrange
		var expected = AllXmlNodes.GetXml(format);
		var input = AllXmlNodes.GetInstance();

		// Act
		var result = input.WriteTo(TestStringBuilderPool.Default, format);

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "IntegrationTest"), Trait("DataFormat", "XML")]
	public void Element_NullName_Throws()
	{
		// Act
		var result1 = () => XmlBuilder.Element(null!, XmlBuilder.Text(null));
		var result2 = () => XmlBuilder.Element(null!, XmlBuilder.Text(null), XmlBuilder.Text(null));
		var result3 = () => XmlBuilder.Element(null!, new List<IXmlNode>{ XmlBuilder.Text(null) });

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
		Trait("Category", "IntegrationTest"), Trait("DataFormat", "XML")]
	public void Element_InvalidName_Throws()
	{
		// Act
		var invalidText = "Some: invalid <text>";

		var result1 = () => XmlBuilder.Element(invalidText, XmlBuilder.Text(null));
		var result2 = () => XmlBuilder.Element(invalidText, XmlBuilder.Text(null), XmlBuilder.Text(null));
		var result3 = () => XmlBuilder.Element(invalidText, new List<IXmlNode> { XmlBuilder.Text(null) });

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
		Trait("Category", "IntegrationTest"), Trait("DataFormat", "XML")]
	public void Attribute_NullName_Throws()
	{
		// Act
		var result = () => XmlBuilder.Attribute(null!, null);

		// Assert
		result
			.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName("name")
			.And
				.StackTrace!.Split(Environment.NewLine).Length
				.Should().Be(8);
	}

	[Fact,
		Trait("Category", "IntegrationTest"), Trait("DataFormat", "XML")]
	public void Attribute_InvalidName_Throws()
	{
		// Act
		var invalidText = "Some: invalid <text>";

		var result = () => XmlBuilder.Attribute(invalidText, null);

		// Assert
		result
			.Should()
			.ThrowExactly<ArgumentException>()
			.WithParameterName("name")
			.And
				.StackTrace!.Split(Environment.NewLine).Length
				.Should().Be(6);
	}
}