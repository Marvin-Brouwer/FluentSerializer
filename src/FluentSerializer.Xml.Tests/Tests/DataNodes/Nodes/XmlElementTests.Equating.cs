using System.Diagnostics;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;

public sealed partial class XmlElementTests
{
	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Equals_OneIsNull_ReturnsFalse() => XmlNodeTests
		.Equating.Equals_OneIsNull_ReturnsFalse(Element(nameof(Element)));

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Equals_DifferentType_ReturnsFalse() => XmlNodeTests
		.Equating.Equals_DifferentType_ReturnsFalse(Element(nameof(Element)));

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Equals_DifferentName_ReturnsFalse() => XmlNodeTests
		.Equating.Equals_DifferentValue_ReturnsFalse(
			Element(nameof(Element)),
			Element(nameof(Attribute))
		);

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Equals_DifferentChildCount_ReturnsFalse() => XmlNodeTests
		.Equating.Equals_DifferentValue_ReturnsFalse(
			Element(nameof(Element)),
			Element(nameof(Element), Text(nameof(Text)))
		);

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Equals_DifferentValue_ReturnsFalse() => XmlNodeTests
		.Equating.Equals_DifferentValue_ReturnsFalse(
			Element(nameof(Element), Text(nameof(Element))),
			Element(nameof(Element), Text(nameof(Text)))
		);

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Equals_AreEqual_ReturnsTrue() => XmlNodeTests
		.Equating.Equals_AreEqual_ReturnsTrue(
			Element(nameof(Element)),
			Element(nameof(Element))
		);
}