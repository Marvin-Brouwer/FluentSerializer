using Ardalis.GuardClauses;

using System.Diagnostics;

namespace FluentSerializer.Json.DataNodes.Nodes;

/// <inheritdoc cref="IJsonComment"/>
[DebuggerDisplay("// {Value,nq}")]
public readonly partial struct JsonCommentSingleLine : IJsonComment
{
	/// <inheritdoc />
	public string Name => JsonCharacterConstants.SingleLineCommentMarker;
	/// <inheritdoc />
	public string? Value { get; }

	/// <inheritdoc cref="JsonBuilder.Comment(in string)"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonBuilder.Comment"/> method instead of this constructor</b>
	/// </remarks>
	public JsonCommentSingleLine(in string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			Value = value;
			return;
		}

		Guard.Against.InvalidFormat(value, nameof(value), @"^([^#\r\n]?.*)", "A single line comment cannot contain newline characters");

		Value = value;
	}
}