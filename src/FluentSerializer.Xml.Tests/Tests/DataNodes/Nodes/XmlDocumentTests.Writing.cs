using FluentAssertions;

using FluentSerializer.Core.TestUtils.Extensions;
using FluentSerializer.Core.TestUtils.Helpers;
using FluentSerializer.Xml.DataNodes.Nodes;

using System;

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

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void AppendTo_DocumentInvalid_Throws()
	{
		// Arrange
		var input = new XmlDocument();

		// Act
		var result = () => input.AppendTo(ref _textWriter, true, 0, false);

		// Assert
		result.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithMessage("The document was is an illegal state, it contains no RootElement *")
			.And // This is mostly here to please Stryker
				.StackTrace!.Split(Environment.NewLine)[1]
				.Should().Contain(nameof(XmlDocument.AppendTo)); 
	}

	[Fact,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void WriteTo_DocumentInvalid_Throws()
	{
		// Arrange
		var input = new XmlDocument();
		// Act
		var result = () => input.WriteTo(TestStringBuilderPool.Default, true, false, 0);

		// Assert
		result.Should()
			.ThrowExactly<ArgumentNullException>()
			.WithMessage("The document was is an illegal state, it contains no RootElement *")
			.And // This is mostly here to please Stryker
				.StackTrace!.Split(Environment.NewLine)[2]
				.Should().Contain(nameof(XmlDocument.WriteTo));
	}
}