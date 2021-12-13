using System;
using System.Globalization;
using FluentSerializer.Xml.Converting.Converters.Base;
using FluentSerializer.Xml.DataNodes;

namespace FluentSerializer.Xml.Converting;

public interface IUseXmlConverters
{
	Func<SimpleTypeConverter<DateTime>> Dates(string? format = null, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.None);
	Func<IXmlConverter<IXmlElement>> Collection(bool wrapCollection = true);
}