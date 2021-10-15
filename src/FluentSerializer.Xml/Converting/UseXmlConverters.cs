using System;
using System.Globalization;
using System.Xml.Linq;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Converting;
using FluentSerializer.Xml.Converting.Converters;
using FluentSerializer.Xml.Converting.Converters.Base;
using FluentSerializer.Xml.Converting.Converters.XNodes;

namespace FluentSerializer.Xml.Converting
{
    public sealed class UseXmlConverters : IUseXmlConverters
    {
        internal static readonly SimpleStructConverter<DateTime> DefaultDateConverter = new DefaultDateConverter();
        internal static readonly IConverter<XElement> WrappedCollectionConverter = new WrappedCollectionConverter();
        internal static readonly IConverter<XElement> NonWrappedCollectionConverter = new NonWrappedCollectionConverter();
        internal static readonly IConverter ConvertibleConverter = new ConvertibleConverter();
        internal static readonly IConverter XObjectConverter = new XObjectConverter();

        public Func<SimpleStructConverter<DateTime>> Dates(string? format = null, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.None)
        {
            if (format is null) return () => DefaultDateConverter;

            Guard.Against.NullOrWhiteSpace(format, nameof(format));
            return () => new DateByFormatConverter(format, culture ?? CultureInfo.CurrentCulture, style);
        }

        public Func<IConverter<XElement>> Collection(bool wrapCollection = true)
        {
            if (wrapCollection) return () => WrappedCollectionConverter;
            return () => NonWrappedCollectionConverter;
        }
    }
}
