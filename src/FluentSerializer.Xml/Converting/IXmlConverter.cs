using System.Xml.Linq;
using FluentSerializer.Core.Converting;

namespace FluentSerializer.Xml.Converting
{
    public interface IXmlConverter<TDataContainer> : IConverter<TDataContainer>, IXmlConverter
        where TDataContainer : XObject
    {
    }
    public interface IXmlConverter : IConverter
    {
    }
}
