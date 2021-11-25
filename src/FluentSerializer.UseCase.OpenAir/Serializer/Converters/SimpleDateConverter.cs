using System.Globalization;
using FluentSerializer.Xml.Converting.Converters;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Converters
{
    public sealed class SimpleDateConverter : DateByFormatConverter
    {
        private const string DateFormat = "yyyy-MM-dd";
        private static readonly CultureInfo CultureInfo = CultureInfo.InvariantCulture;
        private static readonly DateTimeStyles DateTimeStyle = DateTimeStyles.AssumeUniversal;

        public SimpleDateConverter() : base(DateFormat, CultureInfo, DateTimeStyle) { }
    }
}