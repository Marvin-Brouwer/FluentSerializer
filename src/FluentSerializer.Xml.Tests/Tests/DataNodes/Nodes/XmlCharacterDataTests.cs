using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.DataNodes.Nodes;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;

public sealed partial class XmlCharacterDataTests
{
	private const string XmlCharacterDataValue = "\n<p>\n\t69\n\t</p>\n";

	private ITextWriter _textWriter;

	public XmlCharacterDataTests()
	{
		_textWriter = TestStringBuilderPool.CreateSingleInstance();
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeCharacterData_ChildrenNull_ReturnsEmpty()
	{
		// Arrange
		var expected = CData(string.Empty);
		var input = (string?)null!;

		// Act
		var result = new XmlCharacterData(in input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeCharacterData_ChildrenEmpty_ReturnsEmpty()
	{
		// Arrange
		var expected = CData(string.Empty);
		var input = string.Empty;

		// Act
		var result = new XmlCharacterData(in input);

		// Assert
		result.Should().BeEquatableTo(expected);
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void InitializeCharacterData_HasValue_ReturnsWithValue()
	{
		// Arrange
		var expected = CData(XmlCharacterDataValue);

		// Act
		var result = new XmlCharacterData(XmlCharacterDataValue);

		// Assert
		result.Should().BeEquatableTo(expected);
	}
}