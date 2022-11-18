using FluentAssertions;

using FluentSerializer.Core.Constants;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Xml.DataNodes.Nodes;

using System;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;

public sealed partial class XmlCharacterDataTests
{
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void ParseXml_Valid_ReturnsCharacterData()
	{
		// Arrange
		var expected = CData(XmlCharacterDataValue);
		var input = $"<![CDATA[{XmlCharacterDataValue}]]>";

		// Act
		var offset = 0;
		var result = new XmlCharacterData(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML"),
		InlineData(" "), InlineData("  "), InlineData("\t"),
		InlineData(LineEndings.LineFeed), InlineData(LineEndings.CarriageReturn),
		InlineData(LineEndings.ReturnLineFeed)]
	public void ParseXml_ValidWithWhiteSpace_ReturnsCharacterData(string space)
	{
		// Arrange
		var expected = CData(XmlCharacterDataValue);
		// No whitespace at start supported
		var input = $"<![CDATA[{XmlCharacterDataValue}]]>{space}";

		// Act
		var offset = 0;
		var result = new XmlCharacterData(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void ParseXml_Empty_ReturnsCharacterData()
	{
		// Arrange
		var expected = CData(string.Empty);
		var input = "<![CDATA[]]>";

		// Act
		var offset = 0;
		var result = new XmlCharacterData(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void ParseXml_Incomplete_Throws()
	{
		// Arrange
		// Arrange
		var input = $"<![CDATA[{XmlCharacterDataValue}";

		// Act
		var offset = 0;
		var result = () => new XmlCharacterData(input, ref offset);

		// Assert
		result.Should()
			.ThrowExactly<ArgumentOutOfRangeException>();
	}
}