using FluentSerializer.Core.DataNodes;
using Microsoft.Extensions.ObjectPool;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

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

	public override string ToString()
	{
		var stringBuilder = new StringBuilder();
		AppendTo(stringBuilder, false);
		return stringBuilder.ToString();
	}

	public void WriteTo(ObjectPool<StringBuilder> stringBuilders, TextWriter writer, bool format = true, bool writeNull = true, int indent = 0)
	{
		var stringBuilder = stringBuilders.Get();

		var encoding = writer.Encoding;
		stringBuilder
			.Append($"<?xml version=\"1.0\" encoding=\"{encoding.WebName}\"?>")
			.AppendOptionalNewline(format);

		stringBuilder = AppendTo(stringBuilder, format, indent, writeNull);
		writer.Write(stringBuilder);

		stringBuilder.Clear();
		stringBuilders.Return(stringBuilder);
	}

	public StringBuilder AppendTo(StringBuilder stringBuilder, bool format = true, int indent = 0, bool writeNull = true)
	{
		return RootElement?.AppendTo(stringBuilder, format, indent, writeNull) ?? stringBuilder;
	}

	#region IEquatable

	public override bool Equals(object? obj) => obj is IDataNode node && Equals(node);

	public bool Equals(IDataNode? other) => other is IXmlNode node && Equals(node);

	public bool Equals(IXmlNode? other) => DataNodeComparer.Default.Equals(this, other);

	public override int GetHashCode() => DataNodeComparer.Default.GetHashCodeForAll(TypeHashCode, RootElement);

	#endregion
}