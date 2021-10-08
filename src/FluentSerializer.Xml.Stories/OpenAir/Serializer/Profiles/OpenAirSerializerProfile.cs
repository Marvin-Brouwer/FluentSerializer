using FluentSerializer.Xml.Configuration;
using System.Globalization;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Profiles
{
    public abstract class OpenAirSerializerProfile : XmlSerializerProfile
    {
        protected static readonly CustomFieldNamingStrategy CustomFieldNamingStrategy = new CustomFieldNamingStrategy();

        protected static readonly StringBitBooleanConverter StringBitBooleanConverter = new StringBitBooleanConverter();
        protected static readonly OpenAirDateConverter OpenAirDateConverter = new OpenAirDateConverter();
        protected static readonly NonWrappedListConverter NonWrappedListConverter = new NonWrappedListConverter();

        protected DateByFormatConverter SimpleDateConverter
        {
            get
            {
                const string dateFormat = "yyyy-MM-dd";
                var cultureInfo = CultureInfo.InvariantCulture;
                var dateTimeStyle = DateTimeStyles.AssumeUniversal;

                return DateByFormatConverter(dateFormat, cultureInfo, dateTimeStyle);
            }
        }
    }
}