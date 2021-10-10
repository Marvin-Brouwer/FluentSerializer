using FluentSerializer.Xml.Converters.Base;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Converters.XNodes
{
    public sealed class XCommentConverter : SimpleTypeConverter<XComment>
    {
        protected override XComment ConvertToDataType(string value) => new XComment(value);
        protected override string ConvertToString(XComment value) => value.Value;
    }
}