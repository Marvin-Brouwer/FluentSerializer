using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Converting;
using FluentSerializer.Core.Extensions;
using FluentSerializer.Xml.Converting;
using FluentSerializer.Xml.DataNodes;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.UseCase.OpenAir.Serializer.Converters
{
    public class OpenAirDateConverter : IXmlConverter<IXmlElement>
    {
        public SerializerDirection Direction { get; } = SerializerDirection.Both;
        public bool CanConvert(Type targetType) => typeof(DateTime).IsAssignableFrom(targetType);


        object? IConverter<IXmlElement>.Deserialize(IXmlElement elementToSerialize, ISerializerContext context)
        {
            if (!elementToSerialize.Children.Any()) return null;
            var dateWrapper = elementToSerialize.GetChildElement("Date");
            if (dateWrapper is null || !dateWrapper.Children.Any()) return null;

            var yearValue = dateWrapper.GetChildElement("year")?.GetTextValue();
            if (string.IsNullOrWhiteSpace(yearValue))
                throw new DataMisalignedException("A date object is required to have at least a year element");
            var monthValue = dateWrapper.GetChildElement("month")?.GetTextValue();
            if (string.IsNullOrWhiteSpace(monthValue))
                throw new DataMisalignedException("A date object is required to have at least a month element");
            var dayValue = dateWrapper.GetChildElement("day")?.GetTextValue();
            if (string.IsNullOrWhiteSpace(dayValue))
                throw new DataMisalignedException("A date object is required to have at least a day element");

            var hourValue = dateWrapper.GetChildElement("hour")?.GetTextValue();
            if (string.IsNullOrWhiteSpace(hourValue)) return DateTime.ParseExact(
                $"{yearValue}/{monthValue}/{dayValue}", "yyyy/MM/dd",
                CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal
            );

            var minuteValue = dateWrapper.GetChildElement("minute")?.GetTextValue();
            if (string.IsNullOrWhiteSpace(minuteValue))
                throw new DataMisalignedException("A date object with time is required to have at least a minute element");
            var secondValue = dateWrapper.GetChildElement("second")?.GetTextValue();
            if (string.IsNullOrWhiteSpace(secondValue))
                throw new DataMisalignedException("A date object with time is required to have at least a second element");

            return DateTime.ParseExact(
                $"{yearValue}/{monthValue}/{dayValue} {hourValue}:{minuteValue}:{secondValue}", "yyyy/MM/dd HH:mm:ss",
                CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal
            );
        }

        IXmlElement? IConverter<IXmlElement>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            if (objectToSerialize is not DateTime dateToSerialize)
                throw new NotSupportedException($"Cannot convert type '{objectToSerialize.GetType()}'");

            var elementName = context.NamingStrategy.SafeGetName(context.Property, context);
            return Element(elementName, GenerateDateObject(dateToSerialize));
        }

        private static IXmlElement GenerateDateObject(DateTime dateToSerialize)
        {
            var dateProperties = new List<IXmlElement> {
                Element("year", Text(dateToSerialize.ToString("yyyy"))),
                Element("month", Text(dateToSerialize.ToString("MM"))),
                Element("day", Text(dateToSerialize.ToString("dd")))
            };

            if (dateToSerialize.TimeOfDay.TotalSeconds == 0)
                return Element("Date", dateProperties); ;

            dateProperties.Add(Element("hour", Text(dateToSerialize.ToString("HH"))));
            dateProperties.Add(Element("minute", Text(dateToSerialize.ToString("mm"))));
            dateProperties.Add(Element("second", Text(dateToSerialize.ToString("ss"))));

            return Element("Date", dateProperties);
        }
    }
}