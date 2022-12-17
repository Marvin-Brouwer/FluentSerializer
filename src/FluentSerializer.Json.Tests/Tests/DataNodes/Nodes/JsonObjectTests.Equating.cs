using System.Diagnostics;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;

public sealed partial class JsonObjectTests
{
	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_OneIsNull_ReturnsFalse() => JsonNodeTests
		.Equating.Equals_OneIsNull_ReturnsFalse(Object());

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_DifferentType_ReturnsFalse() => JsonNodeTests
		.Equating.Equals_DifferentType_ReturnsFalse(Object());

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_DifferentChildCount_ReturnsFalse() => JsonNodeTests
		.Equating.Equals_DifferentValue_ReturnsFalse(Object(), Object(Property(nameof(Property), Value(null))));

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_DifferentValue_ReturnsFalse() => JsonNodeTests
		.Equating.Equals_DifferentValue_ReturnsFalse(
			Object(Property(nameof(Property), Value(null))),
			Object(Property(nameof(Object), Value(null)))
		);

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_AreEqual_ReturnsTrue() => JsonNodeTests
		.Equating.Equals_AreEqual_ReturnsTrue(Object(), Object());
}