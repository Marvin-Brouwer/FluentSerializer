namespace FluentSerializer.Json.DataNodes
{
    /// <summary>
    /// A representation of a JSON array <br/>
    /// <see href="https://www.json.org/json-en.html#:~:text=An%20array%20is%20an%20ordered%20collection%20of%20values">
    /// https://www.json.org/json-en.html
    /// </see> <br/><br/>
    /// <code>[ ]</code>
    /// </summary>
    public interface IJsonArray : IJsonContainer<IJsonArray>, IJsonArrayContent, IJsonPropertyContent { }
}