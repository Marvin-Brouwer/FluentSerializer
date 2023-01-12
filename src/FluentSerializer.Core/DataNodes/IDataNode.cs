using FluentSerializer.Core.Text;

using Microsoft.Extensions.ObjectPool;

using System;

namespace FluentSerializer.Core.DataNodes;

/// <summary>
/// Generic representation of a serializable data node
/// </summary>
public interface IDataNode : IEquatable<IDataNode?>
{
	/// <summary>
	/// The name of this data node
	/// </summary>
	string Name { get; }

	/// <summary>
	/// Serialize this <see cref="IDataNode"/> to string
	/// </summary>
	string WriteTo(in ObjectPool<ITextWriter> stringBuilders, in bool format = true, in bool writeNull = true, in int indent = 0);

	/// <summary>
	/// Append the serialized value of this <see cref="IDataNode"/> to an <see cref="ITextWriter"/>
	/// </summary>
	ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true);

	/// <summary>
	/// The <see cref="HashCode"/> referring to this node's distinct properties
	/// </summary>
	HashCode GetNodeHash();
}