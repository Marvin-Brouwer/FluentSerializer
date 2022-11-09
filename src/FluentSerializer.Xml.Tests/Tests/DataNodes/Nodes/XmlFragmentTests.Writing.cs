using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Xml.DataNodes.Nodes;

using Xunit;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;
public sealed partial class XmlFragmentTests
{
	[Fact,
		Trait("Category", "UnitTest"),	Trait("DataFormat", "XML")]
	public void AppendTo_HasValue_FormatWriteNull_ReturnsValue()
	{
		// Arrange
		var input = new XmlFragment(XmlFragmentValue);
		// The initial indent is supposed to be added by the container element
		var expected = "<node>69</node>\n\t<node>420</node>";

		// Act
		input.AppendTo(ref _textWriter, true, 0, true);
		var result = _textWriter.ToString();

		// Assert
		result.ShouldBeBinaryEquatableTo(expected);
	}
}