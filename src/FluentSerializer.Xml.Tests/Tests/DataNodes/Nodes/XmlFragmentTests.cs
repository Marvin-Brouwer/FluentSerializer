using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.DataNodes.Nodes;
using System.Collections.Generic;
using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;
public sealed class XmlFragmentTests
{
	private static readonly IEnumerable<IXmlElement> XmlFragmentValue = new List<IXmlElement> {
		Element("node", Text("69")),
		Element("node", Text("420"))
	};

	private ITextWriter _textWriter;

	public XmlFragmentTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}

	#region Parse
	// Xml fragments have no parsing by nature
	#endregion

	#region ToString
	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
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
	#endregion
}