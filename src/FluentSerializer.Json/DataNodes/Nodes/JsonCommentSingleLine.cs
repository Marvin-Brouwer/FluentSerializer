using System;
using Ardalis.GuardClauses;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.Configuration;
using System.Diagnostics;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;

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
		Guard.Against.InvalidFormat(value, nameof(value), @"^([^#\r\n]?.*)", "A single line comment cannot contain newline characters");

		Value = value;
	}
}