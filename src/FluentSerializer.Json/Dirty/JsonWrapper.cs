using System.Text.Json;

namespace FluentSerializer.Json.Dirty
{
    /// <summary>
    /// This wrapper is necessary for the <see cref="FluentSerializer.Json.Converting.IJsonConverter"/> to confirm to the nullability constraints on the interface.
    /// C#9 supports nullable structs in the definition of the interface but as it turns out that does not allow for nullable return types when implementing.
    /// Because of that we decided to constraint the converters to classes and wrap the <see cref="System.Text.Json.JsonElement"/>
    /// </summary>
    public sealed class JsonWrapper
    {
        public JsonWrapper(JsonElement jsonElement)
        {
            JsonElement = jsonElement;
        }

        public JsonElement JsonElement { get; }

        public override string ToString() => JsonElement.ToString();
    }
}
