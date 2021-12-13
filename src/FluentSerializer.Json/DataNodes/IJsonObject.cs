namespace FluentSerializer.Json.DataNodes;

/// <summary>
/// A representation of a JSON object <br/>
/// <see href="https://www.json.org/json-en.html#:~:text=An%20object%20is%20an%20unordered%20set%20of%20name/value%20pairs">
/// https://www.json.org/json-en.html
/// </see> <br/><br/>
/// <code>{ }</code>
/// </summary>
public interface IJsonObject : IJsonContainer<IJsonObject>, IJsonArrayContent, IJsonPropertyContent
{
	IJsonProperty? GetProperty(string name);
}