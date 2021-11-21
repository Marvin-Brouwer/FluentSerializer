using FluentSerializer.Json.DataNodes;
using System.Linq;
using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Profiling.Data
{
    public readonly struct JsonDataCollection
    {
        public static readonly IJsonObject Data = Object(Property("data", Array(/* ArrayPlaceHolder */)));

        /* MethodPlaceHolder */

        static JsonDataCollection()
        {
            // Make sure "remove all uncessasary usings" doesn't remove linq;
            _ = Enumerable.Empty<bool>().Any();
        }
    }
}
