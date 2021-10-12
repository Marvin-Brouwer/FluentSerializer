using System.Globalization;
using FluentSerializer.UseCase.OpenAir.Serializer.Converters;
using FluentSerializer.UseCase.OpenAir.Serializer.NamingStrategies;
using FluentSerializer.Xml.Converters;
using FluentSerializer.Xml.Profiles;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Profiles.Base
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