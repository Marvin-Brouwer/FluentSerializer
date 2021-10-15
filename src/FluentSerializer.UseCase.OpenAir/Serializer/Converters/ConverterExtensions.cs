using System.Xml.Linq;
using FluentSerializer.Xml.Converting;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Converters
{
    public static class ConverterExtensions
    {
        private static readonly OpenAirDateConverter OpenAirDateConverter = new();
        private static readonly RequestTypeValueConverter RequestTypeValueConverter = new();
        private static readonly StringBitBooleanConverter StringBitBooleanConverter = new();
        private static readonly SimpleDateConverter SimpleDateConverter = new();

        public static OpenAirDateConverter OpenAirDate (this IUseXmlConverters _) => OpenAirDateConverter;
        public static IXmlConverter<XAttribute> RequestTypeValue (this IUseXmlConverters _) => RequestTypeValueConverter;
        public static StringBitBooleanConverter StringBitBoolean (this IUseXmlConverters _) => StringBitBooleanConverter;
        public static SimpleDateConverter SimpleDate(this IUseXmlConverters _) => SimpleDateConverter;
    }
}
