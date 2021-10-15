using System;
using System.Globalization;
using System.Xml.Linq;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.Converting;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Converters
{
    public class OpenAirDateConverter : IXmlConverter<XElement>
    {
        public SerializerDirection Direction => SerializerDirection.Both;
        public bool CanConvert(Type targetType) => typeof(DateTime).IsAssignableFrom(targetType);


        object? IConverter<XElement>.Deserialize(XElement elementToSerialize, ISerializerContext context)
        {
            if (elementToSerialize.IsEmpty) return null;
            var dateWrapper = elementToSerialize.Element("Date");
            if (dateWrapper is null || dateWrapper.IsEmpty) return null;

            var yearValue = dateWrapper.Element("year")?.Value;
            if (string.IsNullOrWhiteSpace(yearValue))
                throw new DataMisalignedException("A date object is required to have at least a year element");
            var monthValue = dateWrapper.Element("month")?.Value;
            if (string.IsNullOrWhiteSpace(monthValue))
                throw new DataMisalignedException("A date object is required to have at least a month element");
            var dayValue = dateWrapper.Element("day")?.Value;
            if (string.IsNullOrWhiteSpace(dayValue))
                throw new DataMisalignedException("A date object is required to have at least a day element");

            var hourValue = dateWrapper.Element("hour")?.Value;
            if (string.IsNullOrWhiteSpace(hourValue)) return DateTime.ParseExact(
                $"{yearValue}/{monthValue}/{dayValue}", "yyyy/MM/dd",
                CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal
            );

            var minuteValue = dateWrapper.Element("minute")?.Value;
            if (string.IsNullOrWhiteSpace(minuteValue))
                throw new DataMisalignedException("A date object with time is required to have at least a minute element");
            var secondValue = dateWrapper.Element("second")?.Value;
            if (string.IsNullOrWhiteSpace(secondValue))
                throw new DataMisalignedException("A date object with time is required to have at least a second element");

            return DateTime.ParseExact(
                $"{yearValue}/{monthValue}/{dayValue} {hourValue}:{minuteValue}:{secondValue}", "yyyy/MM/dd HH:mm:ss",
                CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal
            );
        }

        XElement? IConverter<XElement>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            if (!(objectToSerialize is DateTime dateToSerialize))
                throw new NotSupportedException($"Cannot convert type '{objectToSerialize.GetType()}'");

            var elementName = context.NamingStrategy.SafeGetName(context.Property, context);
            return new XElement(elementName, GenerateDateObject(dateToSerialize));
        }

        private static XElement GenerateDateObject(DateTime dateToSerialize)
        {
            var dateWrapper = new XElement("Date",
                            new XElement("year", dateToSerialize.ToString("yyyy")),
                            new XElement("month", dateToSerialize.ToString("MM")),
                            new XElement("day", dateToSerialize.ToString("dd"))
                        );

            if (dateToSerialize.TimeOfDay.TotalSeconds == 0)
                return dateWrapper;

            dateWrapper.Add(
                new XElement("hour", dateToSerialize.ToString("HH")),
                new XElement("minute", dateToSerialize.ToString("mm")),
                new XElement("second", dateToSerialize.ToString("ss"))
            );

            return dateWrapper;
        }
    }
}