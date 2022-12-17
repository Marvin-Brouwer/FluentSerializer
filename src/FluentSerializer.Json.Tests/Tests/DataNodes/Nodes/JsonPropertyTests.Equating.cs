using System.Diagnostics;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;

public sealed partial class JsonPropertyTests
{
	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_OneIsNull_ReturnsFalse() => JsonNodeTests
		.Equating.Equals_OneIsNull_ReturnsFalse(Property(nameof(Property), Value(null)));

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_DifferentType_ReturnsFalse() => JsonNodeTests
		.Equating.Equals_DifferentType_ReturnsFalse(Property(nameof(Property), Value(null)));

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_DifferentName_ReturnsFalse() => JsonNodeTests
		.Equating.Equals_DifferentValue_ReturnsFalse(
			Property(nameof(Property), Value(null)),
			Property(nameof(Object), Value(null))
		);

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_DifferentValue_ReturnsFalse() => JsonNodeTests
		.Equating.Equals_DifferentValue_ReturnsFalse(
			Property(nameof(Property), Value(nameof(Value))),
			Property(nameof(Property), Value(nameof(Object)))
		);

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_DifferentHasValue_ReturnsFalse() => JsonNodeTests
		.Equating.Equals_DifferentValue_ReturnsFalse(
			Property(nameof(Property), Value(null)),
			Property(nameof(Property), Value(nameof(Object)))
		);

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_AreEqual_ReturnsTrue() => JsonNodeTests
		.Equating.Equals_AreEqual_ReturnsTrue(
			Property(nameof(Property), Value(null)),
			Property(nameof(Property), Value(null))
		);
}