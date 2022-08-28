using Ardalis.GuardClauses;

using FluentSerializer.Core.Extensions;
using FluentSerializer.Core.Text;
using FluentSerializer.Xml.Configuration;

namespace FluentSerializer.Xml.DataNodes.Nodes;

public readonly partial struct XmlAttribute
{
	/// <inheritdoc />
	public override string ToString() => this.ToString(XmlSerializerConfiguration.Default);

	/// <inheritdoc />
	public ITextWriter AppendTo(ref ITextWriter stringBuilder, in bool format = true, in int indent = 0, in bool writeNull = true)
	{
		Guard.Against.NullOrWhiteSpace(Name, message: "The attribute was is an illegal state, it contains no Name"
#if NETSTANDARD2_1
			, parameterName: nameof(format)
#endif
		);


		if (!writeNull && string.IsNullOrEmpty(Value)) return stringBuilder;

		stringBuilder
			.Append(Name)
			.Append(XmlCharacterConstants.PropertyAssignmentCharacter)
			.Append(XmlCharacterConstants.PropertyWrapCharacter);

		if (Value is not null) stringBuilder = stringBuilder.Append(Value);

		stringBuilder
			.Append(XmlCharacterConstants.PropertyWrapCharacter);

		return stringBuilder;
	}
}