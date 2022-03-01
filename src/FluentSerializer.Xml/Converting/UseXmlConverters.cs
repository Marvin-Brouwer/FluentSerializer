using System;
using System.Globalization;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Converting.Converters;
using FluentSerializer.Xml.Converting.Converters;
using FluentSerializer.Xml.Converting.Converters.Base;
using FluentSerializer.Xml.DataNodes;

namespace FluentSerializer.Xml.Converting;

/// <inheritdoc/>
public sealed class UseXmlConverters : IUseXmlConverters
{
	internal static readonly SimpleTypeConverter<DateTime> DefaultDateConverter = new DefaultDateConverter();
	internal static readonly IXmlConverter<IXmlElement> WrappedCollectionConverter = new WrappedCollectionConverter();
	private static readonly IXmlConverter<IXmlElement> NonWrappedCollectionConverter = new NonWrappedCollectionConverter();
	internal static readonly IXmlConverter ConvertibleConverter = new ConvertibleConverter();
	internal static readonly IXmlConverter DefaultEnumConverter = new EnumConverter(EnumFormat.Default);

	/// <inheritdoc/>
	public SimpleTypeConverter<DateTime> DateTime() => DefaultDateConverter;

	/// <inheritdoc/>
	public Func<SimpleTypeConverter<DateTime>> DateTime(string format, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.None)
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

	/// <inheritdoc />
	public IXmlConverter Enum() => DefaultEnumConverter;
	/// <inheritdoc />
	public Func<IXmlConverter> Enum(EnumFormat format)
	{
		return () => new EnumConverter(format);
	}
}