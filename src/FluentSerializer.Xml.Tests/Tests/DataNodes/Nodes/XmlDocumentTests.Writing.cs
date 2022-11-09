using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Xml.DataNodes.Nodes;

using Xunit;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;
public sealed partial class XmlDocumentTests
{
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void AppendTo_HasValue_FormatWriteNull_ReturnsValue()
	{
		// Arrange
		var input = new XmlDocument(XmlDocumentValue);
		var expected = XmlDocumentValue.ToString()!;

		// Act
		input.AppendTo(ref _textWriter, true, 0, true);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void WriteTo_HasValue_FormatWriteNull_ReturnsValue()
	{
		// Arrange
		var input = new XmlDocument(XmlDocumentValue);
		var expected = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\n{XmlDocumentValue}";
		var builderPool = TestStringBuilderPool.Default;

		// Act
		var result = input.WriteTo(in builderPool, true, true, 0);

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}
}