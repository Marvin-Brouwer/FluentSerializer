using FluentSerializer.Xml.Converters;
using FluentSerializer.Xml.Profiles;
using FluentSerializer.Xml.Stories.OpenAir.Serializer.Converters;
using FluentSerializer.Xml.Stories.OpenAir.Serializer.NamingStrategies;
using System.Globalization;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Profiles
{
    public abstract class OpenAirSerializerProfile : SerializerProfile
    {
        protected static readonly CustomFieldNamingStrategy CustomFieldNamingStrategy = new CustomFieldNamingStrategy();

        protected static readonly StringBitBooleanConverter StringBitBooleanConverter = new StringBitBooleanConverter();
        protected static readonly OpenAirDateConverter OpenAirDateConverter = new OpenAirDateConverter();

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