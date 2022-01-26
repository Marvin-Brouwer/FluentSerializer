using FluentSerializer.Core.DataNodes;
using FluentSerializer.Xml.Configuration;
using Microsoft.Extensions.ObjectPool;
using System.Collections.Generic;
using System.Diagnostics;
using FluentSerializer.Core.Text;
using FluentSerializer.Core.Text.Extensions;

namespace FluentSerializer.Xml.DataNodes.Nodes;

/// <inheritdoc cref="IXmlDocument"/>
[DebuggerDisplay(DocumentName)]
public readonly struct XmlDocument : IXmlDocument
{
	private static readonly int TypeHashCode = typeof(XmlDocument).GetHashCode();

	public IXmlElement? RootElement { get; }
	public IReadOnlyList<IXmlNode> Children => RootElement?.Children ?? new List<IXmlNode>(0);

	private const string DocumentName = "<?xml ?>";
	public string Name => DocumentName;

	/// <inheritdoc cref="IXmlDocument"/>
	/// <param name="root">The root element to represent the actual document</param>
	public XmlDocument(IXmlElement? root)
	{
		RootElement = root;
	}

	public override string ToString() => this.ToString(XmlSerializerConfiguration.Default);

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

	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		return RootElement?.AppendTo(ref stringBuilder, in format, in indent, in writeNull) ?? stringBuilder;
	}

	#region IEquatable

	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

	public bool Equals(IXmlNode? other) => DataNodeComparer.Default.Equals(this, other);

	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, RootElement);

	#endregion
}