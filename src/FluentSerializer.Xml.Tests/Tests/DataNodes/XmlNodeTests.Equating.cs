using FluentAssertions;

using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.DataNodes;

using Microsoft.Extensions.ObjectPool;

using System;

namespace FluentSerializer.Xml.Tests.Tests.DataNodes;

internal static class XmlNodeTests
{
	internal static class Equating
	{
		internal static void Equals_AreEqual_ReturnsTrue<TXmlNode>(TXmlNode left, TXmlNode right)
			where TXmlNode : IXmlNode
		{
			// Act
			var result = left.Equals(right);

			// Assert
			result.Should().BeTrue();
		}

		internal static void Equals_DifferentType_ReturnsFalse<TXmlNode>(TXmlNode targetNode)
			where TXmlNode : IXmlNode
		{
			// Arrange
			var otherNode = new TestNode();

			// Act
			var result = targetNode.Equals(otherNode);

			// Assert
			result.Should().BeFalse();
		}

		internal static void Equals_DifferentValue_ReturnsFalse<TXmlNode>(TXmlNode left, TXmlNode right)
			where TXmlNode : IXmlNode
		{
			// Act
			var result = left.Equals(right);

			// Assert
			result.Should().BeFalse();
		}

		internal static void Equals_OneIsNull_ReturnsFalse<TXmlNode>(TXmlNode targetNode)
			where TXmlNode : IXmlNode
		{
			// Arrange
			var otherNode = (IXmlNode?)null;

			// Act
			var result = targetNode.Equals(otherNode);

			// Assert
			result.Should().BeFalse();
		}
	}

	private sealed class TestNode : IXmlNode
	{
		public string Name => nameof(TestNode);

#pragma warning disable S3877 // Exceptions should not be thrown from unexpected methods
		public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true) => throw new NotSupportedException("Out of test scope");
		public string WriteTo(in ObjectPool<ITextWriter> stringBuilders, in bool format = true, in bool writeNull = true, in int indent = 0) => throw new NotSupportedException("Out of test scope");

		public bool Equals(IDataNode? other) => throw new NotSupportedException("Out of test scope");
		public bool Equals(IXmlNode? other) => throw new NotSupportedException("Out of test scope");

		public HashCode GetNodeHash() => new();
#pragma warning restore S3877 // Exceptions should not be thrown from unexpected methods
	}
}
