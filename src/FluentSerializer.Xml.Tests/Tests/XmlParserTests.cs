using FluentSerializer.Core.TestUtils.Extensions;
using Xunit;

namespace FluentSerializer.Xml.Tests.Tests;

/// <summary>
/// Testing all nodes combined for the <see cref="XmlParser"/>
/// </summary>
public sealed class XmlParserTests
{
	[Theory,
		Trait("Category", "IntegrationTest"), Trait("DataFormat", "XML"),
		InlineData(true), InlineData(false)]
	public void AllElements_EqualsExpectedInstanceTree(bool format)
	{
		// Arrange
		var expected = AllXmlNodes.GetInstance();
		var input = AllXmlNodes.GetXml(format);

		// Act
		var result = XmlParser.Parse(input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}
}