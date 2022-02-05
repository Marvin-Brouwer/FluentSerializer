using System;
using System.Globalization;
using Ardalis.GuardClauses;
using FluentSerializer.Xml.Converting.Converters;
using FluentSerializer.Xml.Converting.Converters.Base;
using FluentSerializer.Xml.DataNodes;

namespace FluentSerializer.Xml.Converting;

/// <inheritdoc/>
public sealed class UseXmlConverters : IUseXmlConverters
{
	internal static readonly SimpleTypeConverter<DateTime> DefaultDateConverter = new DefaultDateConverter();
	internal static readonly IXmlConverter<IXmlElement> WrappedCollectionConverter = new WrappedCollectionConverter();
	internal static readonly IXmlConverter<IXmlElement> NonWrappedCollectionConverter = new NonWrappedCollectionConverter();
	internal static readonly IXmlConverter ConvertibleConverter = new ConvertibleConverter();

	/// <inheritdoc/>
	public SimpleTypeConverter<DateTime> Dates() => DefaultDateConverter;

	/// <inheritdoc/>
	public Func<SimpleTypeConverter<DateTime>> Dates(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.None)
	{
		Guard.Against.NullOrWhiteSpace(format, nameof(format));
		return () => new DateByFormatConverter(format, culture ?? CultureInfo.CurrentCulture, style);
	}

	/// <inheritdoc/>
	public Func<IXmlConverter<IXmlElement>> Collection(in bool wrapCollection = true)
	{
		if (wrapCollection) return () => WrappedCollectionConverter;
		return () => NonWrappedCollectionConverter;
	}
}