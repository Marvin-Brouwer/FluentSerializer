using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;
using FluentSerializer.Json.Configuration;

using Microsoft.Extensions.ObjectPool;

namespace FluentSerializer.Json.DataNodes.Nodes;

public readonly partial struct JsonCommentSingleLine
{
	/// <inheritdoc />
	public override string ToString() => this.ToString(JsonSerializerConfiguration.Default);

	/// <inheritdoc />
	public string WriteTo(in ObjectPool<ITextWriter> stringBuilders, in bool format = true, in bool writeNull = true, in int indent = 0) =>
		DataNodeExtensions.WriteTo(this, in stringBuilders, in format, in writeNull, in indent);

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		// JSON does not support empty property assignment or array members
		if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

		const char spacer = ' ';

		// Fallback because otherwise JSON wouldn't be readable
		if (!format)
			return stringBuilder
				.Append(JsonCharacterConstants.MultiLineCommentStart)
				.Append(spacer)
				.Append(Value)
				.Append(spacer)
				.Append(JsonCharacterConstants.MultiLineCommentEnd);

		return stringBuilder
			.Append(JsonCharacterConstants.SingleLineCommentMarker)
			.Append(spacer)
			.Append(Value);
	}
}