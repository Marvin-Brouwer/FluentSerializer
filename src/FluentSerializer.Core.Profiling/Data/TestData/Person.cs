using FluentSerializer.Core.Data.Json;
using FluentSerializer.Core.Data.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentSerializer.Core.Profiling.Data.TestData
{
    public sealed class Person
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public Bogus.DataSets.Name.Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        public IJsonContainer ToJsonElement()
        {
            var properties = new List<JsonProperty> {
                new JsonProperty("fullName", JsonValue.String(string.Join(" ", FirstName, MiddleName, LastName))),
                new JsonProperty("details",
                    new JsonObject(
                        new JsonProperty("firstName", JsonValue.String(FirstName)),
                        new JsonProperty("middleName", MiddleName is null ? null : JsonValue.String(MiddleName)),
                        new JsonProperty("lastName", JsonValue.String(LastName)),
                        new JsonProperty("gender", JsonValue.String(Gender.ToString().ToLowerInvariant())),
                        new JsonProperty("dob", JsonValue.String(DateOfBirth.ToString("yyyy/MM/dd")))
                    )
                )
            };

            return new JsonObject(properties);
        }

        public IXmlContainer ToXmlElement()
        {
            var children = new List<IXmlNode> {
                new XmlElement("fullName", new XmlText($"{FirstName} {LastName}")),
                new XmlElement("Details",
                    new XmlElement("firstName", new XmlText(FirstName)),
                    new XmlElement("middleName", MiddleName is null ? null : new XmlText(MiddleName)),
                    new XmlElement("lastName", new XmlText(LastName)),
                    new XmlElement("gender", new XmlText(Gender.ToString().ToLowerInvariant())),
                    new XmlElement("dob", new XmlText(DateOfBirth.ToString("yyyy/MM/dd")))
                )
            };

            return new XmlElement(nameof(Person), children);
        }
    }
}
