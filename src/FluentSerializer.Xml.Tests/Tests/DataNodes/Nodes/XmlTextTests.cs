using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.DataNodes.Nodes;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;

public sealed partial class XmlTextTests
{
	private const string XmlTextValue = "69\n69";

	private ITextWriter _textWriter;

	public XmlTextTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeText_ChildrenNull_ReturnsEmpty()
	{
		// Arrange
		var expected = Text(string.Empty);
		var input = (string?)null!;

		// Act
		var result = new XmlText(in input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeText_ChildrenEmpty_ReturnsEmpty()
	{
		// Arrange
		var expected = Text(string.Empty);
		var input = string.Empty;

		// Act
		var result = new XmlText(in input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeText_HasValue_ReturnsWithValue()
	{
		// Arrange
		var expected = Text(XmlTextValue);

		// Act
		var result = new XmlText(XmlTextValue);

		// Assert
		result.Should().BeEquatableTo(expected);
	}
}