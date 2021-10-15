using System;
using System.Globalization;
using System.Xml.Linq;
using FluentSerializer.Core.Converting;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.Converting.Converters.Base;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Converters
{
    public static class ConverterExtensions
    {
        private static readonly SimpleStructConverter<DateTime> SimpleDateConverter = ConfigureSimpleDateConverter();
        private static SimpleStructConverter<DateTime> ConfigureSimpleDateConverter()
        {
            const string dateFormat = "yyyy-MM-dd";
            var cultureInfo = CultureInfo.InvariantCulture;
            var dateTimeStyle = DateTimeStyles.AssumeUniversal;

            return Converter.For.Dates(dateFormat, cultureInfo, dateTimeStyle)();
        }

        public static OpenAirDateConverter OpenAirDate (this IUseXmlConverters _) => new OpenAirDateConverter();
        public static IConverter<XAttribute> RequestTypeValue (this IUseXmlConverters _) => new RequestTypeValueConverter();
        public static StringBitBooleanConverter StringBitBoolean (this IUseXmlConverters _) => new StringBitBooleanConverter();
        public static SimpleStructConverter<DateTime> SimpleDate(this IUseXmlConverters _) => SimpleDateConverter;
    }
}
