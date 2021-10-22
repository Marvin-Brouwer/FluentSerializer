using System;
using System.Collections.Generic;
using System.Linq;
using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Xml.DataNodes;

namespace FluentSerializer.Xml.Profiling.Data
{
    public readonly struct XmlDataSet
    {
        public static List<XmlElement> XmlValues { get; }

        static XmlDataSet()
        {
            Console.WriteLine("Mapping XML dataSet");
            var xmlDataSet = TestDataSet.TestData
                .Select(testItem => testItem.ToXmlElement()).ToList();

            XmlValues = new List<XmlElement>(3){
                new ("Data", xmlDataSet.Take(xmlDataSet.Count / 4).ToList()),
                new ("Data", xmlDataSet.Take(xmlDataSet.Count / 2).ToList()),
                new ("Data", xmlDataSet)
            };
        }
    }
}
