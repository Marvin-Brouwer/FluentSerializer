using System;
using System.Collections.Generic;
using System.Linq;
using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Json.DataNodes;

using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Profiling.Data
{
    public readonly struct JsonDataSet
    {
        public static List<IJsonObject> JsonValues { get; }

        static JsonDataSet()
        {
            Console.WriteLine("Mapping JSON dataSet");
            var jsonDataSet = TestDataSet.TestData
                .Select(testItem => testItem.ToJsonElement()).ToList();

            JsonValues = new List<IJsonObject>(3){
                Object(Property("data", Array(jsonDataSet.Take(jsonDataSet.Count / 4).ToList()))),
                Object(Property("data", Array(jsonDataSet.Take(jsonDataSet.Count / 2).ToList()))),
                Object(Property("data", Array(jsonDataSet)))
            };
        }
    }
}
