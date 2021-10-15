using System;
using System.Globalization;
using System.Xml.Linq;
using FluentSerializer.Xml.Converting.Converters.Base;

namespace FluentSerializer.Xml.Converting
{
    public interface IUseXmlConverters
    {
        Func<SimpleTypeConverter<DateTime>> Dates(string? format = null, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.None);
        Func<IXmlConverter<XElement>> Collection(bool wrapCollection = true);
    }
}
