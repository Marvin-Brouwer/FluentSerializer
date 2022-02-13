using FluentSerializer.Xml.Configuration;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Xml.DataNodes.Nodes;


public readonly partial struct XmlText
{
	/// <inheritdoc />
	public override string ToString() => this.ToString(XmlSerializerConfiguration.Default);

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		// This should never happen because null tags are self-closing but just to be sure this check is here
		if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

		return stringBuilder.Append(Value);
	}
}