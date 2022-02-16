using FluentSerializer.Core.DataNodes;
using FluentSerializer.Json.Configuration;
using System;
using System.Diagnostics;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;

namespace FluentSerializer.Json.DataNodes.Nodes;

public readonly partial struct JsonValue
{
	/// <inheritdoc />
	public override string ToString() => this.ToString(JsonSerializerConfiguration.Default);

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		// JSON does not support empty property assignment or array members
		return stringBuilder.Append(Value ?? JsonCharacterConstants.NullValue);
	}
}