using System.Diagnostics;

using Xunit;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Tests.Tests.DataNodes.Nodes;

public sealed partial class JsonValueTests
{
	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_OneIsNull_ReturnsFalse() => JsonNodeTests
		.Equating.Equals_OneIsNull_ReturnsFalse(Value(null));

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_DifferentType_ReturnsFalse() => JsonNodeTests
		.Equating.Equals_DifferentType_ReturnsFalse(Value(null));

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_DifferentValue_ReturnsFalse() => JsonNodeTests
		.Equating.Equals_DifferentValue_ReturnsFalse(Value(nameof(Value)), Value(nameof(Comment)));

	[Fact, DebuggerStepThrough,
		Trait("Category", "UnitTest"), Trait("DataFormat", "JSON")]
	public void Equals_AreEqual_ReturnsTrue() => JsonNodeTests
		.Equating.Equals_AreEqual_ReturnsTrue(Value(nameof(Value)), Value(nameof(Value)));
}