using System;
using System.Globalization;
using System.Xml.Linq;
using FluentSerializer.Core.Converting;
using FluentSerializer.Xml.Converting.Converters.Base;

namespace FluentSerializer.Xml.Converting
{
    public interface IUseXmlConverters
    {
        Func<SimpleStructConverter<DateTime>> Dates(string? format = null, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.None);
        Func<IConverter<XElement>> Collection(bool wrapCollection = true);
    }
}
