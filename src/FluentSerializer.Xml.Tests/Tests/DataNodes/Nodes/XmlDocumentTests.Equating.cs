using FluentSerializer.Xml.DataNodes.Nodes;

using System.Diagnostics;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;

public sealed partial class XmlDocumentTests
{
	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Equals_OneIsNull_ReturnsFalse() => XmlNodeTests
		.Equating.Equals_OneIsNull_ReturnsFalse(new XmlDocument(Element(nameof(Element))));

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Equals_BothAreNull_ReturnsTrue() => XmlNodeTests
		.Equating.Equals_AreEqual_ReturnsTrue(
			new XmlDocument(null),
			new XmlDocument(null)
		);

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Equals_DifferentType_ReturnsFalse() => XmlNodeTests
		.Equating.Equals_DifferentType_ReturnsFalse(new XmlDocument(Element(nameof(Element))));

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Equals_DifferentValue_ReturnsFalse() => XmlNodeTests
		.Equating.Equals_DifferentValue_ReturnsFalse(
			new XmlDocument(Element(nameof(Element))),
			new XmlDocument(Element(nameof(Text)))
		);

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Equals_AreEqual_ReturnsTrue() => XmlNodeTests
		.Equating.Equals_AreEqual_ReturnsTrue(
			new XmlDocument(Element(nameof(Element))),
			new XmlDocument(Element(nameof(Element)))
		);
}