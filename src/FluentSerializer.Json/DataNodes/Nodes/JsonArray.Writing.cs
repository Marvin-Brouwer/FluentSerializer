using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;
using FluentSerializer.Json.Configuration;

using Microsoft.Extensions.ObjectPool;

namespace FluentSerializer.Json.DataNodes.Nodes;

public readonly partial struct JsonArray
{
	/// <inheritdoc />
	public override string ToString() => this.ToString(JsonSerializerConfiguration.Default);

	/// <inheritdoc />
	public string WriteTo(in ObjectPool<ITextWriter> stringBuilders, in bool format = true, in bool writeNull = true, in int indent = 0) =>
		DataNodeExtensions.WriteTo(this, in stringBuilders, in format, in writeNull, in indent);

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		var childIndent = indent + 1;
		var currentChildIndex = 0;

		stringBuilder
			.Append(JsonCharacterConstants.ArrayStartCharacter);

		foreach (var child in Children)
		{
			stringBuilder
				.AppendOptionalNewline(in format)
				.AppendOptionalIndent(in childIndent, in format)
				.AppendNode(child, in format, in childIndent, in writeNull);

			// Make sure the last item does not append a comma to confirm to JSON spec.
			if (child is not IJsonComment && !currentChildIndex.Equals(_lastNonCommentChildIndex))
				stringBuilder.Append(JsonCharacterConstants.DividerCharacter);

			currentChildIndex.Increment();
		}

		stringBuilder
			.AppendOptionalNewline(in format)
			.AppendOptionalIndent(in indent, format)
			.Append(JsonCharacterConstants.ArrayEndCharacter);

		return stringBuilder;
	}
}