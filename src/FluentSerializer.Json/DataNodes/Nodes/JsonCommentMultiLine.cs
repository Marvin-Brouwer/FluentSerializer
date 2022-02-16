using System;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.Configuration;
using System.Diagnostics;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;

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