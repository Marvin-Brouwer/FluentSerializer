using FluentSerializer.Core.Data.Json;
using FluentSerializer.Core.Data.Xml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSerializer.Core.Profiling.Data.TestData
{
    public sealed class House
    {

        public string Type { get; set; }
        public string StreetName { get; set; }
        public int HouseNumber { get; set; }

        public List<Person> Residents { get; set; }

        public JsonObject ToJsonElement()
        {
            var properties = new List<JsonProperty> {
                new JsonProperty("type", JsonValue.String(Type)),
                new JsonProperty("address",
                    new JsonObject(
                        new JsonProperty("street", JsonValue.String(StreetName)),
                        new JsonProperty("number", new JsonValue(HouseNumber.ToString()))
                    )
                ),
                new JsonProperty("residents", new JsonArray(
                    Residents.Select(person => person.ToJsonElement())
                ))
            };

            return new JsonObject(properties);
        }

        public XmlElement ToXmlElement()
        {
            var children = new List<IXmlNode> {
                new XmlAttribute("type", Type),
                new XmlElement("Address",
                    new XmlElement("street", new XmlText(StreetName)),
                    new XmlElement("number", new XmlText(HouseNumber.ToString()))
                ),
                new XmlElement("Residents", 
                    Residents.Select(person => person.ToXmlElement())
                )
            };

            return new XmlElement(nameof(House), children);
        }
    }
}
