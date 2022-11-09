using Ardalis.GuardClauses;

using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.Configuration;

using System.Linq;

namespace FluentSerializer.Xml.DataNodes.Nodes;

public readonly partial struct XmlFragment
{
	/// <inheritdoc />
	public override string ToString() => this.ToString(XmlSerializerConfiguration.Default);

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		Guard.Against.Null(_innerElement, message: "The fragment was is an illegal state, it contains no Element"
#if NETSTANDARD2_1
			, parameterName: nameof(_innerElement)
#endif
		);

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