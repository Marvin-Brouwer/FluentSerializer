using System.Diagnostics;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;

public sealed partial class JsonCommentMultilineTests
{
	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_OneIsNull_ReturnsFalse() => JsonNodeTests
		.Equating.Equals_OneIsNull_ReturnsFalse(MultilineComment(nameof(MultilineComment)));

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_DifferentType_ReturnsFalse() => JsonNodeTests
		.Equating.Equals_DifferentType_ReturnsFalse(MultilineComment(nameof(MultilineComment)));

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_DifferentValue_ReturnsFalse() => JsonNodeTests
		.Equating.Equals_DifferentValue_ReturnsFalse(MultilineComment(nameof(MultilineComment)), MultilineComment(nameof(Object)));

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_AreEqual_ReturnsTrue() => JsonNodeTests
		.Equating.Equals_AreEqual_ReturnsTrue(MultilineComment(nameof(MultilineComment)), MultilineComment(nameof(MultilineComment)));
}