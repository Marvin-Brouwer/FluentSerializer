using System.Diagnostics;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;

public sealed partial class XmlAttributeTests
{
	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Equals_OneIsNull_ReturnsFalse() => XmlNodeTests
		.Equating.Equals_OneIsNull_ReturnsFalse(Attribute(nameof(Attribute), nameof(Text)));

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Equals_DifferentType_ReturnsFalse() => XmlNodeTests
		.Equating.Equals_DifferentType_ReturnsFalse(Attribute(nameof(Attribute), nameof(Text)));

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Equals_DifferentName_ReturnsFalse() => XmlNodeTests
		.Equating.Equals_DifferentValue_ReturnsFalse(
			Attribute(nameof(Attribute), nameof(Text)),
			Attribute(nameof(Element), nameof(Text))
		);

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Equals_DifferentValue_ReturnsFalse() => XmlNodeTests
		.Equating.Equals_DifferentValue_ReturnsFalse(
			Attribute(nameof(Attribute), nameof(Text)),
			Attribute(nameof(Attribute), nameof(Element))
		);

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Equals_AreEqual_ReturnsTrue() => XmlNodeTests
		.Equating.Equals_AreEqual_ReturnsTrue(
			Attribute(nameof(Attribute), nameof(Text)),
			Attribute(nameof(Attribute), nameof(Text))
		);
}