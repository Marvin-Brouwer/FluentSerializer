using FluentAssertions;

using FluentSerializer.Core.TestUtils.Extensions;

using System;

using Xunit;

namespace FluentSerializer.Xml.Tests.Tests;

/// <summary>
/// Testing all nodes combined for the <see cref="XmlParser"/>
/// </summary>
public sealed class XmlParserTests
{
	[Theory,
		Trait("Category", "IntegrationTest"),	Trait("DataFormat", "XML"),
		InlineData(true), InlineData(false)]
	public void AllElements_EqualsExpectedInstanceTree(bool format)
	{
		// Arrange
		var expected = AllXmlNodes.GetInstance();
		var input = AllXmlNodes.GetXml(format);

		// Act
		var result = XmlParser.Parse(input);

		// Assert
		result.Should().BeEquatableTo(expected, format);
	}

	[Fact,
		Trait("Category", "IntegrationTest"), Trait("DataFormat", "XML")]
	public void AllElements_NullValue_Throws()
	{
		// Arrange
		var value = (string)null!;

		// Act
		var result = () => XmlParser.Parse(value);

		// Assert
		result.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(value));
	}

	[Fact,
		Trait("Category", "IntegrationTest"), Trait("DataFormat", "XML")]
	public void AllElements_EmptyValue_Throws()
	{
		// Arrange
		var value = string.Empty;

		// Act
		var result = () => XmlParser.Parse(value);

		// Assert
		result.Should()
			.ThrowExactly<ArgumentException>()
			.WithParameterName(nameof(value));
	}
}