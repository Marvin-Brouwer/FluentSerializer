using FluentSerializer.Core.Constants;
using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Xml.DataNodes.Nodes;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;

public sealed partial class XmlTextTests
{
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void ParseXml_Valid_ReturnsText()
	{
		// Arrange
		var expected = Text(XmlTextValue);
		var input = $"{XmlTextValue}<";

		// Act
		var offset = 0;
		var result = new XmlText(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Theory,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML"),
		InlineData(" "), InlineData("  "), InlineData("\t"),
		InlineData(LineEndings.LineFeed), InlineData(LineEndings.CarriageReturn),
		InlineData(LineEndings.ReturnLineFeed)]
	public void ParseXml_ValidWithWhiteSpace_ReturnsText(string space)
	{
		// Arrange
		var expected = Text(XmlTextValue);
		var input = $"{space}{XmlTextValue}{space}<";

		// Act
		var offset = 0;
		var result = new XmlText(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void ParseXml_Empty_ReturnsText()
	{
		// Arrange
		var expected = Text(string.Empty);
		var input = "<";

		// Act
		var offset = 0;
		var result = new XmlText(input, ref offset);

		// Assert
		result.Should().BeEquatableTo(expected);
	}
}