using System.Xml.Linq;
using FluentSerializer.Xml.Converting;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Converters
{
    public static class ConverterExtensions
    {
        private static readonly OpenAirDateConverter OpenAirDateConverter = new OpenAirDateConverter();
        private static readonly RequestTypeValueConverter RequestTypeValueConverter = new RequestTypeValueConverter();
        private static readonly StringBitBooleanConverter StringBitBooleanConverter = new StringBitBooleanConverter();
        private static readonly SimpleDateConverter SimpleDateConverter = new SimpleDateConverter();

        public static OpenAirDateConverter OpenAirDate (this IUseXmlConverters _) => OpenAirDateConverter;
        public static IXmlConverter<XAttribute> RequestTypeValue (this IUseXmlConverters _) => RequestTypeValueConverter;
        public static StringBitBooleanConverter StringBitBoolean (this IUseXmlConverters _) => StringBitBooleanConverter;
        public static SimpleDateConverter SimpleDate(this IUseXmlConverters _) => SimpleDateConverter;
    }
}
