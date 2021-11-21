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
        private static readonly int _actualSize;

        public static List<DataContainer<IJsonObject>> JsonValues { get; }

        public static IEnumerable<DataContainer<string>> JsonStringValues()
        {
            yield return new DataContainer<string>(File.ReadAllText($"./JsonData.{_actualSize / 4}.json"), _actualSize / 4);
            yield return new DataContainer<string>(File.ReadAllText($"./JsonData.{_actualSize / 2}.json"), _actualSize / 2);
            yield return new DataContainer<string>(File.ReadAllText($"./JsonData.{_actualSize}.json"), _actualSize);
        } 

        static JsonDataSet()
        {
            var testData = new TestDataSet(30000);
            _actualSize = testData.TestData.Count;

            Console.WriteLine("Mapping JSON dataSet");
            var jsonDataSet = testData.TestData
                .Select(testItem => testItem.ToJsonElement()).ToList();

            JsonValues = new List<DataContainer<IJsonObject>>(3){
                new (Object(Property("data", Array(jsonDataSet.Take(jsonDataSet.Count / 4).ToList()))), jsonDataSet.Count / 4),
                new (Object(Property("data", Array(jsonDataSet.Take(jsonDataSet.Count / 2).ToList()))), jsonDataSet.Count / 2),
                new (Object(Property("data", Array(jsonDataSet))), jsonDataSet.Count)
            };

            var stringBuilder = StringBuilderPool.Get();
            GenerateCompiledTestData(jsonDataSet.Count / 4, testData, stringBuilder);
            GenerateCompiledTestData(jsonDataSet.Count / 2, testData, stringBuilder);
            GenerateCompiledTestData(jsonDataSet.Count, testData, stringBuilder);

            //JsonStringValues = new List<DataContainer<string>>(3) {
            //    CreateStringPair(JsonValues[0], stringBuilder, true),
            //    CreateStringPair(JsonValues[1], stringBuilder, true),
            //    CreateStringPair(JsonValues[2], stringBuilder, true)
            //};
            StringBuilderPool.Return(stringBuilder);
        }

        private static void GenerateCompiledTestData(int take, TestDataSet testData, StringBuilder stringBuilder)
        {
            if (!File.Exists($"./JsonDataCollection.{take}.cs"))
            {
                Console.WriteLine($"Writing static data for {take} items");
                File.Copy("../../../Data/JsonDataCollection.cs", $"./JsonDataCollection.{take}.cs");

                using var stringWriter = new StringWriter(stringBuilder);

                stringBuilder.AppendLine(
                    "private static System.Collections.Generic.IEnumerable<IJsonObject> GetObject() => " +
                    "new System.Collections.Generic.List<IJsonObject>{");

                var totalItems = testData.TestData.Take(take).ToList();
                foreach (var item in totalItems)
                {
                    item.ToStringRepresentation(stringBuilder);

                    if (!item.Equals(totalItems[^1]))
                    {
                        stringBuilder.AppendLine(",");
                    }
                }

                stringBuilder.AppendLine("};");

                string text = File.ReadAllText($"./JsonDataCollection.{take}.cs");
                text = text.Replace("/* ArrayPlaceHolder */", "GetObject()");
                text = text.Replace("/* MethodPlaceHolder */", stringBuilder.ToString());
                File.WriteAllText($"./JsonDataCollection.{take}.cs", text);

                var jsonObject = Object(Property("data", Array(totalItems.Select(item => item.ToJsonElement()))));
                using var writer = File.CreateText($"./JsonData.{take}.json");
                jsonObject.WriteTo(StringBuilderPool, writer, true, false, 0);
                writer.Flush();
                writer.Close();
            }
        }

        private static DataContainer<string> CreateStringPair(DataContainer<IJsonObject> jsonObject, StringBuilder stringBuilder, bool format)
        {
            using var writer = new StringWriter(stringBuilder);

            jsonObject.Value.WriteTo(StringBuilderPool, writer, format);
            writer.Flush();
            var json = stringBuilder.ToString();
            stringBuilder.Clear();

            return new DataContainer<string>(json, jsonObject.Size);
        }
    }
}
