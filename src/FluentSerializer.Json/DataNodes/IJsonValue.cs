using FluentSerializer.Core.DataNodes;

namespace FluentSerializer.Json.DataNodes
{
    /// <summary>
    /// A representation of a JSON value <br/>
    /// <see href="https://www.json.org/json-en.html#:~:text=A%20value%20can%20be%20a%20string%20in%20double%20quotes%2C%20or">
    /// https://www.json.org/json-en.html
    /// </see>
    /// </summary>
    public interface IJsonValue : IDataValue, IJsonNode, IJsonPropertyContent
    {
        bool HasValue { get; }
    }
}
