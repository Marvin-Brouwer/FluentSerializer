using FluentSerializer.Xml.Converting;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Converters
{
    public static class ConverterExtensions
    {
        private static readonly OpenAirDateConverter OpenAirDateConverter = new();
        private static readonly RequestTypeValueConverter RequestTypeValueConverter = new();
        private static readonly StringBitBooleanConverter StringBitBooleanConverter = new();
        private static readonly SimpleDateConverter SimpleDateConverter = new();

		/// <inheritdoc cref="Converters.OpenAirDateConverter"/>
		public static OpenAirDateConverter OpenAirDate(this IUseXmlConverters _) => OpenAirDateConverter;
		/// <inheritdoc cref="Converters.RequestTypeValueConverter"/>
		public static RequestTypeValueConverter RequestTypeValue(this IUseXmlConverters _) => RequestTypeValueConverter;
		/// <inheritdoc cref="Converters.StringBitBooleanConverter"/>
		public static StringBitBooleanConverter StringBitBoolean (this IUseXmlConverters _) => StringBitBooleanConverter;
		/// <inheritdoc cref="Converters.SimpleDateConverter"/>
		public static SimpleDateConverter SimpleDate(this IUseXmlConverters _) => SimpleDateConverter;
    }
}
