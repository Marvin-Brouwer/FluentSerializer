using System.Diagnostics;

using Xunit;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes.Nodes;

public sealed partial class XmlTextTests
{
	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Equals_OneIsNull_ReturnsFalse() => XmlNodeTests
		.Equating.Equals_OneIsNull_ReturnsFalse(Text(nameof(Text)));

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Equals_DifferentType_ReturnsFalse() => XmlNodeTests
		.Equating.Equals_DifferentType_ReturnsFalse(Text(nameof(Text)));

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Equals_DifferentValue_ReturnsFalse() => XmlNodeTests
		.Equating.Equals_DifferentValue_ReturnsFalse(
			Text(nameof(Text)),
			Text(nameof(Comment))
		);

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "XML")]
	public void Equals_AreEqual_ReturnsTrue() => XmlNodeTests
		.Equating.Equals_AreEqual_ReturnsTrue(
			Text(nameof(Text)),
			Text(nameof(Text))
		);
}