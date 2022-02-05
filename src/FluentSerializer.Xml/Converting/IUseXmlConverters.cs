using System;
using System.Globalization;
using FluentSerializer.Xml.Converting.Converters;
using FluentSerializer.Xml.Converting.Converters.Base;
using FluentSerializer.Xml.DataNodes;

namespace FluentSerializer.Xml.Converting;

/// <summary>
/// Use an <see cref="IXmlConverter"/> for this mapping
/// </summary>
/// <remarks>
/// For using a custom <see cref="IXmlConverter"/> create an extension method on <see cref="IUseXmlConverters"/>
/// </remarks>
public interface IUseXmlConverters
{
	/// <inheritdoc cref="DefaultDateConverter" />
	SimpleTypeConverter<DateTime> Dates();

	/// <inheritdoc cref="DateByFormatConverter" />
	Func<SimpleTypeConverter<DateTime>> Dates(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.None);

	/// <summary>
	/// Converts most DotNet collections
	/// </summary>
	/// <param name="wrapCollection">When true, wraps the collection in a tag of the property name</param>
	Func<IXmlConverter<IXmlElement>> Collection(bool wrapCollection = true);

}