using FluentSerializer.Core.DataNodes;

using System.Diagnostics.CodeAnalysis;

namespace FluentSerializer.Json.DataNodes;

/// <summary>
/// A representation of a JSON property <br/>
/// <see href="https://www.json.org/json-en.html#:~:text=An%20object%20is%20an%20unordered%20set%20of%20name/value%20pairs">
/// https://www.json.org/json-en.html
/// </see> <br/><br/>
/// <code>"name": value,</code>
/// </summary>
public interface IJsonProperty : IJsonContainer<IJsonProperty>, IJsonObjectContent {

	/// <inheritdoc cref="IDataValue.Value" />
	IJsonNode? Value { get; }

#if NET5_0_OR_GREATER
	[MemberNotNullWhen(true, nameof(Value))]
#endif
	/// <inheritdoc cref="IJsonValue.HasValue" />
	bool HasValue { get; }
}