using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Json.DataNodes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Profiling.Data
{
    public sealed partial class JsonDataCollection : BaseCollection<JsonDataCollection, IJsonObject>
    {
        public static JsonDataCollection Default = new();
        protected override string GetStringFileName(string name) => $"JsonDataCollection.{name}.json";
        protected override IJsonObject WrapData(List<IJsonObject> data) => Object(Property("data", Array(data)));

        public static void Generate(int smallSize, int mediumSize, int largeSize) =>
            new JsonDataCollection().GenerateData(smallSize, mediumSize, largeSize);

        protected override void GenerateCodeContent(List<ResidentialArea> residentialAreas, StringBuilder stringBuilder)
        {
            foreach (var item in residentialAreas)
            {
                item.ToStringRepresentation(stringBuilder);

                if (!item.Equals(residentialAreas[^1]))
                {
                    stringBuilder.AppendLine(",");
                }
            }
        }

        protected override IEnumerable<IJsonObject> ConvertToData(List<ResidentialArea> residentialAreas) =>
            residentialAreas.Select(area => area.ToJsonElement());
    }
}
