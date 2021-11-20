using System;
using System.Collections.Generic;
using System.Linq;
using FluentSerializer.Core.Dirty;
using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Json.DataNodes;
using Microsoft.Extensions.ObjectPool;
using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Profiling.Data
{

    public readonly struct JsonDataSet
    {
        private static readonly ObjectPoolProvider ObjectPoolProvider = new DefaultObjectPoolProvider();
        public static readonly ObjectPool<StringFast> StringFastPool = ObjectPoolProvider.CreateStringFastPool();

        public static List<DataContainer<IJsonObject>> JsonValues { get; }
        public static List<DataContainer<string>> JsonStringValues { get; }

        static JsonDataSet()
        {
            var testData = new TestDataSet(30000);

            Console.WriteLine("Mapping JSON dataSet");
            var jsonDataSet = testData.TestData
                .Select(testItem => testItem.ToJsonElement()).ToList();

            JsonValues = new List<DataContainer<IJsonObject>>(3){
                new (Object(Property("data", Array(jsonDataSet.Take(jsonDataSet.Count / 4).ToList()))), jsonDataSet.Count / 4),
                new (Object(Property("data", Array(jsonDataSet.Take(jsonDataSet.Count / 2).ToList()))), jsonDataSet.Count / 2),
                new (Object(Property("data", Array(jsonDataSet))), jsonDataSet.Count)
            };

            var stringBuilder = StringFastPool.Get();
            JsonStringValues = new List<DataContainer<string>>(3) {
                CreateStringPair(JsonValues[0], stringBuilder, true),
                CreateStringPair(JsonValues[1], stringBuilder, true),
                CreateStringPair(JsonValues[2], stringBuilder, true)
            };
            StringFastPool.Return(stringBuilder);
        }

        private static DataContainer<string> CreateStringPair(DataContainer<IJsonObject> jsonObject, StringFast stringBuilder, bool format)
        {
            stringBuilder = jsonObject.Value.AppendTo(stringBuilder, format);
            var json = stringBuilder.ToString();
            stringBuilder.Clear();

            return new DataContainer<string>(json, jsonObject.Size);
        }
    }
}
