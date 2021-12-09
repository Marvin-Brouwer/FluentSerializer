using FluentSerializer.Core.BenchmarkUtils.TestData;
using FluentSerializer.Xml.DataNodes;
using System.Collections.Generic;
using System.Linq;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Benchmark.Data
{
    public sealed class XmlDataCollection : DataCollectionFactory<IXmlElement>
    {
        public static XmlDataCollection Default = new();

        protected override string GetStringFileName(int dataCount) => $"{nameof(XmlDataCollection)}-{dataCount}.xml";
        protected override IXmlElement ConvertToData(List<ResidentialArea> data, int residentialAreaCount, long houseCount, long peopleCount) =>
            Element("Data", 
                data.Select(area => area.ToXmlElement())
                .Append(Attribute(nameof(residentialAreaCount), residentialAreaCount.ToString()))
                .Append(Attribute(nameof(houseCount), houseCount.ToString()))
                .Append(Attribute(nameof(peopleCount), peopleCount.ToString()))
            );

        protected override IXmlElement GetDataFromSpan(string stringValue) => XmlParser.Parse(stringValue);
    }
}
