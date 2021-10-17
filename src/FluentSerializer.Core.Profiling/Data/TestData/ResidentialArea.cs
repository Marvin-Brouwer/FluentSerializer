using FluentSerializer.Core.Data.Json;
using FluentSerializer.Core.Data.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentSerializer.Core.Profiling.Data.TestData
{
    public sealed class ResidentialArea
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public List<House> Houses { get; set; }

        public JsonObject ToJsonElement()
        {
            var properties = new List<JsonProperty> {
                new JsonProperty("type", JsonValue.String(Type)),
                new JsonProperty("name", JsonValue.String(Name)),
                new JsonProperty("houses", new JsonArray(
                    Houses.Select(house => house.ToJsonElement())
                ))
            };

            return new JsonObject(properties);
        }

        public XmlElement ToXmlElement()
        {
            var children = new List<IXmlNode> {
                new XmlAttribute("type", Type),
                new XmlElement("name", new XmlText(Name)),
                new XmlElement("Houses", 
                    Houses.Select(house => house.ToXmlElement())
                )
            };

            return new XmlElement(nameof(ResidentialArea), children);
        }
    }
}
