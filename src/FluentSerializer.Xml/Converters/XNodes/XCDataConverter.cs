using FluentSerializer.Xml.Converters.Base;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Converters.XNodes
{
    public sealed class XCDataConverter : SimpleTypeConverter<XCData>
    {
        protected override XCData ConvertToDataType(string value) => new XCData(value);
        protected override string ConvertToString(XCData value) => value.Value;
    }
}