namespace FluentSerializer.Json.DataNodes;

/// <summary>
/// A representation of a JSON property <br/>
/// <see href="https://www.json.org/json-en.html#:~:text=An%20object%20is%20an%20unordered%20set%20of%20name/value%20pairs">
/// https://www.json.org/json-en.html
/// </see> <br/><br/>
/// <code>"name": value,</code>
/// </summary>
public interface IJsonProperty : IJsonContainer<IJsonProperty>, IJsonObjectContent { 
	IJsonNode? Value { get; }
	bool HasValue { get; }
}