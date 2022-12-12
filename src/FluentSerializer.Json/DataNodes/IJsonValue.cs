using FluentSerializer.Core.DataNodes;

using System.Diagnostics.CodeAnalysis;

namespace FluentSerializer.Json.DataNodes;

/// <summary>
/// A representation of a JSON value <br/>
/// <see href="https://www.json.org/json-en.html#:~:text=A%20value%20can%20be%20a%20string%20in%20double%20quotes%2C%20or">
/// https://www.json.org/json-en.html
/// </see>
/// </summary>
public interface IJsonValue : IDataValue, IJsonPropertyContent, IJsonArrayContent
{
	/// <inheritdoc cref="IDataValue.Value" />
	new string? Value { get; }

	/// <summary>
	/// Property indicating whether the value is null or empty
	/// </summary>
#if NET5_0_OR_GREATER
	[MemberNotNullWhen(true, nameof(Value))]
#endif
	bool HasValue { get; }
}