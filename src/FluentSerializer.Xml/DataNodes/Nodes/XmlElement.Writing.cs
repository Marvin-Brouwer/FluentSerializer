using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.Configuration;
using System.Linq;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Xml.DataNodes.Nodes;

public readonly partial struct XmlElement
{
	/// <inheritdoc />
	public override string ToString() => this.ToString(XmlSerializerConfiguration.Default);

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		const char spacer = ' ';

		var children = Children;
		var childIndent = format ? indent + 1 : 0;

		if (!writeNull && !children.Any()) return stringBuilder;
		if (!writeNull && children[0] is IXmlText text
			&& string.IsNullOrEmpty(text.Value)) return stringBuilder;

		stringBuilder
			.Append(XmlCharacterConstants.TagStartCharacter)
			.Append(Name);

		if (!children.Any()) return stringBuilder
			.Append(spacer)
			.Append(XmlCharacterConstants.TagTerminationCharacter)
			.Append(XmlCharacterConstants.TagEndCharacter);

		foreach (var attribute in _attributes)
		{
			stringBuilder
				.AppendOptionalNewline(in format)
				.AppendOptionalIndent(in indent, in format)
				.Append(spacer)
				.AppendNode(attribute, in format, in childIndent, in writeNull);
		}
		stringBuilder
			.Append(XmlCharacterConstants.TagEndCharacter);

		// Technically this object can have multiple text nodes, only the first needs indentation
		var textOnly = true;
		var firstTextNode = true;
		foreach (var child in _children)
		{
			if (child is IXmlElement childElement) {
				textOnly = false;
				firstTextNode = true;

				stringBuilder
					.AppendOptionalNewline(in format)
					.AppendOptionalIndent(in childIndent, in format);

				stringBuilder
					.AppendNode(childElement, in format, in childIndent, in writeNull);

				continue;
			}
			if (child is IXmlText textNode)
			{
				if (firstTextNode)
				{
					firstTextNode = false;
					if (!textOnly) stringBuilder
						.AppendOptionalNewline(in format)
						.AppendOptionalIndent(in childIndent, in format);

					stringBuilder
						.AppendNode(textNode, true, in childIndent, in writeNull);

					continue;
				}

				stringBuilder
					.AppendNode(textNode, false, in childIndent, in writeNull);

				continue;
			}
			if (child is IXmlComment commentNode)
			{
				stringBuilder
					.AppendOptionalNewline(in format)
					.AppendOptionalIndent(in childIndent, in format);

				stringBuilder
					.AppendNode(commentNode, true, in childIndent, in writeNull);

				continue;
			}
			if (child is IXmlCharacterData cDataNode)
			{
				stringBuilder
					.AppendOptionalNewline(in format)
					.AppendOptionalIndent(in childIndent, in format);

				stringBuilder
					.AppendNode(cDataNode, true, in childIndent, in writeNull);
			}
		}

		if (!textOnly) stringBuilder
			.AppendOptionalNewline(in format)
			.AppendOptionalIndent(in indent, in format);

		stringBuilder
			.Append(XmlCharacterConstants.TagStartCharacter)
			.Append(XmlCharacterConstants.TagTerminationCharacter)
			.Append(Name)
			.Append(XmlCharacterConstants.TagEndCharacter);

		return stringBuilder;
	}
}