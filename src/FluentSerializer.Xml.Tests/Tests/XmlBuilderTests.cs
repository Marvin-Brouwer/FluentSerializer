using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;

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
}