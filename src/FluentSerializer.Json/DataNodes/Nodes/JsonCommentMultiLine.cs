using System.Diagnostics;

namespace FluentSerializer.Json.DataNodes.Nodes;

/// <inheritdoc cref="IJsonComment"/>
[DebuggerDisplay("/* {Value,nq} */")]
public readonly partial struct JsonCommentMultiLine : IJsonComment
{
	/// <inheritdoc />
	public string Name => JsonCharacterConstants.SingleLineCommentMarker;
	/// <inheritdoc />
	public string? Value { get; }

	/// <inheritdoc cref="JsonBuilder.MultilineComment(in string)"/>
	/// <remarks>
	/// <b>Please use <see cref="JsonBuilder.MultilineComment"/> method instead of this constructor</b>
	/// </remarks>
	public JsonCommentMultiLine(in string value)
	{
		Value = value;
	}
}