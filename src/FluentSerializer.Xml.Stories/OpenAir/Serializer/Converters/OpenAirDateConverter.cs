using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Services;
using System;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Stories.OpenAir.Serializer.Converters
{
    public class OpenAirDateConverter : IConverter<XElement>
    {
        public SerializerDirection Direction => SerializerDirection.Both;
        public bool CanConvert(Type targetType) => typeof(DateTime).IsAssignableFrom(targetType);


        object? IConverter<XElement>.Deserialize(XElement elementToSerialize, ISerializerContext context)
        {
            throw new System.NotImplementedException();
        }

        XElement? IConverter<XElement>.Serialize(object objectToSerialize, ISerializerContext context)
        {
            if (objectToSerialize is null) return null;
            if (!(objectToSerialize is DateTime dateToSerialize))
                throw new NotSupportedException($"Cannot convert type '{objectToSerialize.GetType()}'");

            var elementName = context.NamingStrategy.GetName(context.Property);
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