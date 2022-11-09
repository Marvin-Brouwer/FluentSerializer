using FluentAssertions;

using FluentSerializer.Core.Constants;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Xml.DataNodes.Nodes;

using System;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;
public sealed partial class XmlAttributeTests
{
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void ParseXml_Valid_ReturnsAttribute()
	{
		// Arrange
		var expected = Attribute(XmlAttributeName, XmlAttributeValue);
		var input = $"attribute=\"{XmlAttributeValue}\"";

		// Act
		var offset = 0;
		var result = new XmlAttribute(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML"),
		InlineData(" "), InlineData("  "), InlineData("\t"),
		InlineData(LineEndings.LineFeed), InlineData(LineEndings.CarriageReturn),
		InlineData(LineEndings.ReturnLineFeed)]
	public void ParseXml_ValidWithWhiteSpace_ReturnsAttribute(string space)
	{
		// Arrange
		var expected = Attribute(XmlAttributeName, XmlAttributeValue);
		var input = $"{space}attribute{space}={space}\"{XmlAttributeValue}\"{space}";

		// Act
		var offset = 0;
		var result = new XmlAttribute(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void ParseXml_Empty_ReturnsAttribute()
	{
		// Arrange
		var expected = Attribute(XmlAttributeName, null);
		var input = "attribute=\"\"";

		// Act
		var offset = 0;
		var result = new XmlAttribute(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void ParseXml_InvalidName_ReturnsAttributeWithInvalidName()
	{
		// Arrange
		var input = "attr!ibute$=\"\"";

		// Act
		var offset = 0;
		var result = new XmlAttribute(input, ref offset);

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveInvalidName();
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void ParseXml_InvalidStructure_ReturnsAttributeWithInvalidName()
	{
		// Arrange
		var input1 = "attribute\"\" attribute2=\"\" />";
		var input2 = "attribute\"\" />";

		// Act
		var offset1 = 0;
		var result1 = new XmlAttribute(input1, ref offset1);
		var offset2 = 0;
		var result2 = new XmlAttribute(input2, ref offset2);

		// Assert
		// These have an invalid name this will result in properties not being found
		// instead of throwing parsing errors
		result1.Name.Should().Be("attribute\"\" attribute2");
		result1.Should().HaveInvalidName();
		result2.Name.Should().Be("attribute\"\"");
		result2.Should().HaveInvalidName();
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void ParseXml_Incomplete_Throws()
	{
		// Arrange
		var input = "attribute";

		// Act
		var offset = 0;
		var result = () => new XmlAttribute(input, ref offset);

		// Assert
		result.Should()
			.ThrowExactly<ArgumentOutOfRangeException>();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void ParseXml_UnTerminatedValue_FailsGracefully()
	{
		// Arrange
		var expected = "The element of surpri...";
		var input = "attribute=\"The element of surpri... >";

		// Act
		var offset = 0;
		var result = new XmlAttribute(input, ref offset);

		// Assert
		result.Value.Should().BeEquivalentTo(expected);
	}
}