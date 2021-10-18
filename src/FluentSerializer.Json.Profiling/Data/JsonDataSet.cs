using System;
using System.Collections.Generic;
using System.Linq;
using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Profiling.Data;

namespace FluentSerializer.Json.Profiling.Data
{
    public readonly struct JsonDataSet
    {
        public static List<JsonObject> JsonValues { get; }

        static JsonDataSet()
        {
            Console.WriteLine("Mapping JSON dataSet");
            var jsonDataSet = TestDataSet.TestData
                .Select(testItem => testItem.ToJsonElement()).ToList();

            JsonValues = new List<JsonObject>(3){
                new (new JsonProperty("data", new JsonArray(jsonDataSet.Take(jsonDataSet.Count / 4).ToList()))),
                new (new JsonProperty("data", new JsonArray(jsonDataSet.Take(jsonDataSet.Count / 2).ToList()))),
                new (new JsonProperty("data", new JsonArray(jsonDataSet)))
            };
        }
    }
}
