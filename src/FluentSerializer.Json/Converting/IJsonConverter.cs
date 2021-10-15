using FluentSerializer.Core.Converting;
using Newtonsoft.Json.Linq;

namespace FluentSerializer.Json.Converting
{
    public interface IJsonConverter : IConverter<JToken>
    {
    }
}
