using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Json.DataNodes;
using Microsoft.Extensions.ObjectPool;
using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Profiling.Data
{

    public readonly struct JsonDataSet
    {
        private static readonly ObjectPoolProvider ObjectPoolProvider = new DefaultObjectPoolProvider();
        public static readonly ObjectPool<StringBuilder> StringBuilderPool = ObjectPoolProvider.CreateStringBuilderPool();

        public static List<DataContainer<IJsonObject>> JsonValues { get; }
        public static List<DataContainer<string>> JsonStringValues { get; }

        static JsonDataSet()
        {
            var testData = new TestDataSet(25000);

            Console.WriteLine("Mapping JSON dataSet");
            var jsonDataSet = testData.TestData
                .Select(testItem => testItem.ToJsonElement()).ToList();

            JsonValues = new List<DataContainer<IJsonObject>>(3){
                new (Object(Property("data", Array(jsonDataSet.Take(jsonDataSet.Count / 4).ToList()))), jsonDataSet.Count / 4),
                new (Object(Property("data", Array(jsonDataSet.Take(jsonDataSet.Count / 2).ToList()))), jsonDataSet.Count / 2),
                new (Object(Property("data", Array(jsonDataSet))), jsonDataSet.Count)
            };

            var stringBuilder = StringBuilderPool.Get();
            JsonStringValues = new List<DataContainer<string>>(3) {
                CreateStringPair(JsonValues[0], stringBuilder, true),
                CreateStringPair(JsonValues[1], stringBuilder, true),
                CreateStringPair(JsonValues[2], stringBuilder, true)
            };
            StringBuilderPool.Return(stringBuilder);
        }

        private static DataContainer<string> CreateStringPair(DataContainer<IJsonObject> jsonObject, StringBuilder stringBuilder, bool format)
        {
            using var writer = new StringWriter(stringBuilder);

            jsonObject.Value.WriteTo(StringBuilderPool, writer, format);
            writer.Flush();
            var json = stringBuilder.ToString();
            stringBuilder.Clear();

            return new (json, jsonObject.Size);
        }
    }
}
