using FluentAssertions;

using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Text;
using FluentSerializer.Json.DataNodes;

using Microsoft.Extensions.ObjectPool;

using System;

namespace FluentSerializer.Json.Tests.Tests.DataNodes;

internal static class JsonNodeTests
{
	internal static class Equating
	{
		internal static void Equals_AreEqual_ReturnsTrue<TJsonNode>(TJsonNode left, TJsonNode right)
			where TJsonNode : IJsonNode
		{
			// Act
			var result = left.Equals(right);

			// Assert
			result.Should().BeTrue();
		}

		internal static void Equals_DifferentType_ReturnsFalse<TJsonNode>(TJsonNode targetNode)
			where TJsonNode : IJsonNode
		{
			// Arrange
			var otherNode = new TestNode();

			// Act
			var result = targetNode.Equals(otherNode);

			// Assert
			result.Should().BeFalse();
		}

		internal static void Equals_DifferentValue_ReturnsFalse<TJsonNode>(TJsonNode left, TJsonNode right)
			where TJsonNode : IJsonNode
		{
			// Act
			var result = left.Equals(right);

			// Assert
			result.Should().BeFalse();
		}

		internal static void Equals_OneIsNull_ReturnsFalse<TJsonNode>(TJsonNode targetNode)
			where TJsonNode : IJsonNode
		{
			// Arrange
			var otherNode = (IJsonNode?)null;

			// Act
			var result = targetNode.Equals(otherNode);

			// Assert
			result.Should().BeFalse();
		}
	}

	private sealed class TestNode : IJsonNode
	{
		public string Name => nameof(TestNode);

#pragma warning disable S3877 // Exceptions should not be thrown from unexpected methods
		public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true) => throw new NotSupportedException("Out of test scope");
		public string WriteTo(in ObjectPool<ITextWriter> stringBuilders, in bool format = true, in bool writeNull = true, in int indent = 0) => throw new NotSupportedException("Out of test scope");

		public bool Equals(IDataNode? other) => throw new NotSupportedException("Out of test scope");
		public bool Equals(IJsonNode? other) => throw new NotSupportedException("Out of test scope");

		public HashCode GetNodeHash() => new();

#pragma warning restore S3877 // Exceptions should not be thrown from unexpected methods
	}
}
