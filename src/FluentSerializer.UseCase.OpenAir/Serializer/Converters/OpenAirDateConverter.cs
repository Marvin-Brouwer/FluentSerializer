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
        public bool CanConvert(in Type targetType) => typeof(DateTime).IsAssignableFrom(targetType);


        object? IConverter<IXmlElement>.Deserialize(in IXmlElement elementToSerialize, in ISerializerContext context)
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

        IXmlElement? IConverter<IXmlElement>.Serialize(in object objectToSerialize, in ISerializerContext context)
        {
            if (objectToSerialize is not DateTime dateToSerialize)
                throw new NotSupportedException($"Cannot convert type '{objectToSerialize.GetType()}'");

            var elementName = context.NamingStrategy.SafeGetName(context.Property, context);
            return Element(elementName, GenerateDateObject(in dateToSerialize));
        }

        private static IXmlElement GenerateDateObject(in DateTime dateToSerialize)
        {
			var universalDate = dateToSerialize.ToUniversalTime();

            var dateProperties = new List<IXmlElement> {
                Element("year", Text(universalDate.ToString("yyyy"))),
                Element("month", Text(universalDate.ToString("MM"))),
                Element("day", Text(universalDate.ToString("dd")))
            };

            if (dateToSerialize.TimeOfDay.TotalSeconds == 0)
                return Element("Date", dateProperties); 

            dateProperties.Add(Element("hour", Text(universalDate.ToString("HH"))));
            dateProperties.Add(Element("minute", Text(universalDate.ToString("mm"))));
            dateProperties.Add(Element("second", Text(universalDate.ToString("ss"))));

            return Element("Date", dateProperties);
        }
    }
}