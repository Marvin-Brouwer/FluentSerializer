using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;
using FluentSerializer.Json.Configuration;

namespace FluentSerializer.Json.DataNodes.Nodes;

public readonly partial struct JsonObject
{
	/// <inheritdoc />
	public override string ToString() => this.ToString(JsonSerializerConfiguration.Default);

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		var childIndent = indent + 1;
		var currentPropertyIndex = 0;

		stringBuilder
			.Append(JsonCharacterConstants.ObjectStartCharacter);

		foreach (var child in Children)
		{
			if (!writeNull && child is IJsonProperty jsonProperty && !jsonProperty.HasValue) continue;

			stringBuilder
				.AppendOptionalNewline(in format)
				.AppendOptionalIndent(in childIndent, in format)
				.AppendNode(child, in format, in childIndent, in writeNull);
                
			// Make sure the last item does not append a comma to confirm to JSON spec.
			if (child is not IJsonComment && !currentPropertyIndex.Equals(_lastPropertyIndex))
				stringBuilder.Append(JsonCharacterConstants.DividerCharacter);

			currentPropertyIndex++;
		}

		stringBuilder
			.AppendOptionalNewline(in format)
			.AppendOptionalIndent(indent, in format)
			.Append(JsonCharacterConstants.ObjectEndCharacter);

		return stringBuilder;
	}
}