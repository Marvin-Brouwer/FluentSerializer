using Ardalis.GuardClauses;

using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;
using FluentSerializer.Json.Configuration;

namespace FluentSerializer.Json.DataNodes.Nodes;

public readonly partial struct JsonProperty
{
	/// <inheritdoc />
	public override string ToString() => this.ToString(JsonSerializerConfiguration.Default);

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		Guard.Against.NullOrWhiteSpace(Name, message: "The property was is an illegal state, it contains no Name"
#if NETSTANDARD2_1
			, parameterName: nameof(Name)
#endif
		);

		const char spacer = ' ';

		if (!writeNull && !HasValue) return stringBuilder;

		var childValue = _children.SingleItem;

		stringBuilder
			.Append(JsonCharacterConstants.PropertyWrapCharacter)
			.Append(Name)
			.Append(JsonCharacterConstants.PropertyWrapCharacter);

		stringBuilder.Append(JsonCharacterConstants.PropertyAssignmentCharacter);
		if (format) stringBuilder.Append(spacer);

		if (!HasValue) stringBuilder.Append(JsonCharacterConstants.NullValue);
		else stringBuilder.AppendNode(childValue, in format, in indent, in writeNull);

		return stringBuilder;
	}
}