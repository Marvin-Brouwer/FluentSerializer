﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentSerializer.Core.Dirty;
using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Xml.DataNodes;
using Microsoft.Extensions.ObjectPool;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Profiling.Data
{

    public readonly struct XmlDataSet
    {
        private static readonly ObjectPoolProvider ObjectPoolProvider = new DefaultObjectPoolProvider();
        public static readonly ObjectPool<StringFast> StringFastPool = ObjectPoolProvider.CreateStringFastPool();

        public static List<DataContainer<IXmlElement>> XmlValues { get; }
        public static List<DataContainer<string>> XmlStringValues { get; }

        static XmlDataSet()
        {
            var testData = new TestDataSet(22000);

            Console.WriteLine("Mapping XML dataSet");
            var xmlDataSet = testData.TestData
                .Select(testItem => testItem.ToXmlElement()).ToList();

            XmlValues = new List<DataContainer<IXmlElement>>(3){

                new (Element("Data", xmlDataSet.Take(xmlDataSet.Count / 4).ToList()), xmlDataSet.Count / 4),
                new (Element("Data", xmlDataSet.Take(xmlDataSet.Count / 2).ToList()), xmlDataSet.Count / 2),
                new (Element("Data", xmlDataSet), xmlDataSet.Count)
            };

            var stringBuilder = StringFastPool.Get();
            XmlStringValues = new List<DataContainer<string>>(3) {
                CreateStringPair(XmlValues[0], stringBuilder, true),
                CreateStringPair(XmlValues[1], stringBuilder, true),
                CreateStringPair(XmlValues[2], stringBuilder, true)
            };
            StringFastPool.Return(stringBuilder);
        }

        private static DataContainer<string> CreateStringPair(DataContainer<IXmlElement> xmlObject, StringFast stringBuilder, bool format)
        {
            stringBuilder = xmlObject.Value.AppendTo(stringBuilder, format);
            var xml = stringBuilder.ToString();
            stringBuilder.Clear();

            return new DataContainer<string>(xml, xmlObject.Size);
        }
    }
}
