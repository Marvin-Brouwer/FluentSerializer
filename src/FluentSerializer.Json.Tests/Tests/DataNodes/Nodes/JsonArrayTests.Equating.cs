using System.Diagnostics;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;

public sealed partial class JsonArrayTests
{
	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_OneIsNull_ReturnsFalse() => JsonNodeTests
		.Equating.Equals_OneIsNull_ReturnsFalse(Array());

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_DifferentType_ReturnsFalse() => JsonNodeTests
		.Equating.Equals_DifferentType_ReturnsFalse(Array());

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_DifferentChildCount_ReturnsFalse() => JsonNodeTests
		.Equating.Equals_DifferentValue_ReturnsFalse(Array(), Array(Array()));

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_DifferentValue_ReturnsFalse() => JsonNodeTests
		.Equating.Equals_DifferentValue_ReturnsFalse(Array(Object()), Array(Array()));

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_AreEqual_ReturnsTrue() => JsonNodeTests
		.Equating.Equals_AreEqual_ReturnsTrue(Array(), Array());
}