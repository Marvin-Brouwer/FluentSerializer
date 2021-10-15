using System;
using System.Globalization;
using System.Xml.Linq;
using Ardalis.GuardClauses;
using FluentSerializer.Xml.Converting.Converters;
using FluentSerializer.Xml.Converting.Converters.Base;
using FluentSerializer.Xml.Converting.Converters.XNodes;

namespace FluentSerializer.Xml.Converting
{
    public sealed class UseXmlConverters : IUseXmlConverters
    {
        internal static readonly SimpleTypeConverter<DateTime> DefaultDateConverter = new DefaultDateConverter();
        internal static readonly IXmlConverter<XElement> WrappedCollectionConverter = new WrappedCollectionConverter();
        internal static readonly IXmlConverter<XElement> NonWrappedCollectionConverter = new NonWrappedCollectionConverter();
        internal static readonly IXmlConverter ConvertibleConverter = new ConvertibleConverter();
        internal static readonly IXmlConverter XObjectConverter = new XObjectConverter();

        public Func<SimpleTypeConverter<DateTime>> Dates(string? format = null, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.None)
        {
            if (format is null) return () => DefaultDateConverter;

            Guard.Against.NullOrWhiteSpace(format, nameof(format));
            return () => new DateByFormatConverter(format, culture ?? CultureInfo.CurrentCulture, style);
        }

        public Func<IXmlConverter<XElement>> Collection(bool wrapCollection = true)
        {
            if (wrapCollection) return () => WrappedCollectionConverter;
            return () => NonWrappedCollectionConverter;
        }
    }
}
