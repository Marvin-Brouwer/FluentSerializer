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

        public static List<JsonObject> JsonValues { get; }
        public static List<XmlElement> XmlValues { get; }

#if (DEBUG)
        private const int LargeSet = 100;
        private const int MiddleSet = 50;
        private const int SmallSet = 10;
#else
        private const int LargeSet = 10000;
        private const int MiddleSet = 5000;
        private const int SmallSet = 2500;
#endif

        static TestDataSet()
        {
            Console.WriteLine("Building test dataSet");
            var testData = BogusConfiguration.Generate(BogusSeed, LargeSet).ToList();

            Console.WriteLine("Mapping JSON dataSet");
            var jsonDataSet = testData
                .Select(testItem => testItem.ToJsonElement()).ToList();

            Console.WriteLine("Mapping XML dataSet");
            var xmlDataSet = testData
                .Select(testItem => testItem.ToXmlElement()).ToList();
            
            Console.WriteLine("Wrapping collection subsets");

            JsonValues = new List<JsonObject>(3){
                new (new JsonProperty("data", new JsonArray(jsonDataSet.Take(SmallSet).ToList()))),
                new (new JsonProperty("data", new JsonArray(jsonDataSet.Take(MiddleSet).ToList()))),
                new (new JsonProperty("data", new JsonArray(jsonDataSet)))
            };

            XmlValues =  new List<XmlElement>(3){
                new ("Data", xmlDataSet.Take(SmallSet).ToList()),
                new ("Data", xmlDataSet.Take(MiddleSet).ToList()),
                new ("Data", xmlDataSet)
            };

            Console.WriteLine("Wrapped collection subsets");
        }
    }
}
