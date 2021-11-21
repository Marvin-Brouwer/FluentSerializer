using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Json.DataNodes;
using System.Collections.Generic;
using System.Linq;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Profiling.Data
{
    public sealed class JsonDataCollection : DataCollectionFactory<IJsonObject>
    {
        public static JsonDataCollection Default = new();

        protected override string GetStringFileName(string name) => $"{nameof(JsonDataCollection)}-{name}.json";
        protected override IJsonObject ConvertToData(List<ResidentialArea> residentialAreas) =>
            Object(Property("data", Array(residentialAreas.Select(area => area.ToJsonElement()))));

    }
}
