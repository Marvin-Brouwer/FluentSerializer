using FluentSerializer.Core.BenchmarkUtils.TestData;
using FluentSerializer.Xml.DataNodes;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Benchmark.Data;

public sealed class XmlBenchmakData : DataCollectionFactory<IXmlElement>
{
	public static readonly XmlBenchmakData Default = new();

	protected override string GetStringFileName(int dataCount) => $"{nameof(XmlBenchmakData)}-{dataCount}.xml";
	protected override IXmlElement ConvertToData(List<ResidentialArea> data, int residentialAreaCount, long houseCount, long peopleCount) =>
		Element("Data", 
			data.Select(area => area.ToXmlElement())
				.Append(Attribute(nameof(residentialAreaCount), residentialAreaCount.ToString(CultureInfo.InvariantCulture)))
				.Append(Attribute(nameof(houseCount), houseCount.ToString(CultureInfo.InvariantCulture)))
				.Append(Attribute(nameof(peopleCount), peopleCount.ToString(CultureInfo.InvariantCulture)))
		);

	protected override IXmlElement GetDataFromSpan(string stringValue) => XmlParser.Parse(stringValue);
}