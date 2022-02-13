using FluentSerializer.Xml.Configuration;
using System.Linq;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Xml.DataNodes.Nodes;

public readonly partial struct XmlFragment
{
	/// <inheritdoc />
	public override string ToString() => this.ToString(XmlSerializerConfiguration.Default);

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		var childIndent = format ? indent + 1 : 0;

		if (!_innerElement.Children.Any()) return stringBuilder;

		var firstNode = true;
		foreach (var child in _innerElement.Children)
		{
			if (!firstNode) stringBuilder
				.AppendOptionalNewline(in format)
				.AppendOptionalIndent(in childIndent, format);

			stringBuilder
				.AppendNode(child, in format, in childIndent, in writeNull);

			firstNode = false;
		}

		return stringBuilder;
	}
}