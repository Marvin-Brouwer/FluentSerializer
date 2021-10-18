using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentSerializer.Core.Data.Json;
using FluentSerializer.Core.Data.Xml;

namespace FluentSerializer.Core.Profiling.Data.TestData
{
    public readonly struct TestDataSet
    {
        private const int BogusSeed = 98123600;

        public List<JsonObject> JsonValues { get; }
        public List<XmlElement> XmlValues { get; }

        public TestDataSet (int largeSet, int middleSet, int smallSet)
        {
            Console.WriteLine("Building test dataSet");
            var testData = BogusConfiguration.Generate(BogusSeed, largeSet).ToList();

            Console.WriteLine("Mapping JSON dataSet");
            var jsonDataSet = testData
                .Select(testItem => testItem.ToJsonElement()).ToList();

            Console.WriteLine("Mapping XML dataSet");
            var xmlDataSet = testData
                .Select(testItem => testItem.ToXmlElement()).ToList();
            
            Console.WriteLine("Wrapping collection subsets");

            JsonValues = new List<JsonObject>(3){
                new (new JsonProperty("data", new JsonArray(jsonDataSet.Take(smallSet).ToList()))),
                new (new JsonProperty("data", new JsonArray(jsonDataSet.Take(middleSet).ToList()))),
                new (new JsonProperty("data", new JsonArray(jsonDataSet)))
            };

            XmlValues =  new List<XmlElement>(3){
                new ("Data", xmlDataSet.Take(smallSet).ToList()),
                new ("Data", xmlDataSet.Take(middleSet).ToList()),
                new ("Data", xmlDataSet)
            };

            Console.WriteLine("Wrapped collection subsets");
        }
    }
}
