using FluentSerializer.Xml.DataNodes;
using System.IO;
using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Tests.Tests;

internal readonly struct AllXmlNodes
{
	internal static IXmlElement GetInstance() => Element("Class",
		Attribute("someAttribute", "1"),
		Comment("Comment"),
		CData("<p>some xml data here</p>"),
		Element("someProperty", Element("AnotherClass")),
		Text("text here")
	);
	internal static string GetXml(bool format) => format ?
		File.ReadAllText("./Tests/AllXmlNodes.Formatted.xml") :
		File.ReadAllText("./Tests/AllXmlNodes.Slim.xml");
}
