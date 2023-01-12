using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.Configuration;

using Microsoft.Extensions.ObjectPool;

namespace FluentSerializer.Xml.DataNodes.Nodes;

public readonly partial struct XmlText
{
	/// <inheritdoc />
	public override string ToString() => this.ToString(XmlSerializerConfiguration.Default);

	/// <inheritdoc />
	public string WriteTo(in ObjectPool<ITextWriter> stringBuilders, in bool format = true, in bool writeNull = true, in int indent = 0) =>
		DataNodeExtensions.WriteTo(this, in stringBuilders, in format, in writeNull, in indent);

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		// This should never happen because null tags are self-closing but just to be sure this check is here
		if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

		return stringBuilder.Append(Value);
	}
}