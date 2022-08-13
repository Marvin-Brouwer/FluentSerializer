using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;
using FluentSerializer.Json.Configuration;

namespace FluentSerializer.Json.DataNodes.Nodes;

public readonly partial struct JsonCommentMultiLine
{
	/// <inheritdoc />
	public override string ToString() => this.ToString(JsonSerializerConfiguration.Default);

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		// JSON does not support empty property assignment or array members
		if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

		const char spacer = ' ';

		return stringBuilder
			.Append(JsonCharacterConstants.MultiLineCommentStart)
			.Append(spacer)
			.Append(Value)
			.Append(spacer)
			.Append(JsonCharacterConstants.MultiLineCommentEnd);
	}
}