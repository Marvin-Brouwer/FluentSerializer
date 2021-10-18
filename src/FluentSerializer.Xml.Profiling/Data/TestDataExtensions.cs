using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Xml.DataNodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSerializer.Xml.Profiling.Data
{
    public static class TestDataExtensions
    {
        public static IXmlContainer ToXmlElement(this ResidentialArea residentialArea)
        {
            var children = new List<IXmlNode> {
                new XmlAttribute("type", residentialArea.Type),
                new XmlElement("name", new XmlText(residentialArea.Name)),
                new XmlElement("Houses",
                    residentialArea.Houses.Select(house => house.ToXmlElement())
                )
            };

            return new XmlElement(nameof(ResidentialArea), children);
        }

        public static IXmlContainer ToXmlElement(this House house)
        {
            var children = new List<IXmlNode> {
                new XmlAttribute("type", house.Type),
                new XmlElement("Address",
                    new XmlElement("street", new XmlText(house.StreetName)),
                    new XmlElement("number", new XmlText(house.HouseNumber.ToString()))
                ),
                new XmlElement("Residents",
                    house.Residents.Select(person => person.ToXmlElement())
                )
            };

            return new XmlElement(nameof(House), children);
        }

        public static IXmlContainer ToXmlElement(this Person person)
        {
            var children = new List<IXmlNode> {
                new XmlElement("fullName", new XmlText(
                    person.MiddleName is null
                        ? string.Join(" ", person.FirstName, person.LastName)
                        : string.Join(" ", person.FirstName, person.MiddleName, person.LastName))),
                new XmlElement("Details",
                    new XmlElement("firstName", new XmlText(person.FirstName)),
                    new XmlElement("middleName", person.MiddleName is null ? null : new XmlText(person.MiddleName)),
                    new XmlElement("lastName", new XmlText(person.LastName)),
                    new XmlElement("gender", new XmlText(person.Gender.ToString().ToLowerInvariant())),
                    new XmlElement("dob", new XmlText(person.DateOfBirth.ToString("yyyy/MM/dd")))
                )
            };

            return new XmlElement(nameof(Person), children);
        }
    }
}
