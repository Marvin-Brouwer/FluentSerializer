using System;
using System.Globalization;
using Ardalis.GuardClauses;
using FluentSerializer.Json.Converting.Converters;

namespace FluentSerializer.Json.Converting
{
    public sealed class UseJsonConverters : IUseJsonConverters
    {
        internal static readonly IJsonConverter DefaultDateConverter = new DefaultDateConverter();
        internal static readonly IJsonConverter CollectionConverter = new CollectionConverter();
        internal static readonly IJsonConverter ConvertibleConverter = new ConvertibleConverter();

        public Func<IJsonConverter> Dates(string? format = null, CultureInfo? culture = null, DateTimeStyles style = DateTimeStyles.None)
        {
            if (format is null) return () => DefaultDateConverter;

            Guard.Against.NullOrWhiteSpace(format, nameof(format));
            return () => new DateByFormatConverter(format, culture ?? CultureInfo.CurrentCulture, style);
        }

        public IJsonConverter Collection() => CollectionConverter;
    }
}
