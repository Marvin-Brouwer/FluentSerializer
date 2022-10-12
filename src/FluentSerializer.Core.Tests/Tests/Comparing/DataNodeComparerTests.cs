using FluentSerializer.Core.Comparing;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Text;

using System;

// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable S3877 // Exceptions should not be thrown from unexpected methods

namespace FluentSerializer.Core.Tests.Tests.Comparing;

public sealed partial class DataNodeComparerTests
{
	public sealed class TestDataNode : IDataNode
	{
		public string Name { get; }
		public string Value { get; init; }

		public TestDataNode(string value)
		{
			Name = value;
			Value = value;
		}

		public override int GetHashCode()
		{
			return DataNodeComparer.Default.GetHashCodeForAll(
				nameof(TestDataNode), Name, Value
			);
		}

		public bool Equals(IDataNode? other)
		{
			throw new NotSupportedException("The equals will be tested through the Comparer");
		}

		public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0,
			in bool writeNull = true)
		{
			throw new NotSupportedException("Out of test scope");
		}
	}
}