using FluentSerializer.Core.DataNodes;
using FluentSerializer.Xml.Configuration;
using Microsoft.Extensions.ObjectPool;
using System.Collections.Generic;
using System.Diagnostics;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Xml.DataNodes.Nodes;

/// <inheritdoc cref="IXmlDocument"/>
[DebuggerDisplay(DocumentName)]
public readonly struct XmlDocument : IXmlDocument
{
	private static readonly int TypeHashCode = typeof(XmlDocument).GetHashCode();

	/// <inheritdoc />
	public IXmlElement? RootElement { get; }
	/// <inheritdoc />
	public IReadOnlyList<IXmlNode> Children => RootElement?.Children ?? new List<IXmlNode>(0);

	private const string DocumentName = "<?xml ?>";
	/// <inheritdoc />
	public string Name => DocumentName;

	/// <inheritdoc cref="IXmlDocument"/>
	/// <param name="root">The root element to represent the actual document</param>
	public XmlDocument(in IXmlElement? root)
	{
		RootElement = root;
	}

	/// <inheritdoc />
	public override string ToString() => this.ToString(XmlSerializerConfiguration.Default);

	/// <inheritdoc />
	public string WriteTo(in ObjectPool<ITextWriter> stringBuilders, in bool format = true, in bool writeNull = true, in int indent = 0)
	{
		var stringBuilder = stringBuilders.Get();

		try
		{
			stringBuilder
				.Append($"<?xml version=\"1.0\" encoding=\"{stringBuilder.TextConfiguration.Encoding.WebName}\"?>")
				.AppendOptionalNewline(in format);

			AppendTo(ref stringBuilder, in format, in indent, in writeNull);
			return stringBuilder.ToString();
		}
		finally
		{
			stringBuilders.Return(stringBuilder);
		}
	}

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		return RootElement?.AppendTo(ref stringBuilder, in format, in indent, in writeNull) ?? stringBuilder;
	}

	#region IEquatable

	/// <inheritdoc />
	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

	/// <inheritdoc />
	public bool Equals(IXmlNode? other) => DataNodeComparer.Default.Equals(this, other);

	/// <inheritdoc />
	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, RootElement);

	#endregion
}