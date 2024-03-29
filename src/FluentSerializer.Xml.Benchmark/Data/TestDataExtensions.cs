using FluentSerializer.Core.BenchmarkUtils.TestData;
using FluentSerializer.Xml.DataNodes;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Benchmark.Data;

public static class TestDataExtensions
{
	public static IXmlNode ToXmlElement(this ResidentialArea residentialArea)
	{
		var children = new List<IXmlNode> {
			Attribute("type", residentialArea.Type),
			Element("name", Text(residentialArea.Name)),
			Element("Houses",
				residentialArea.Houses.Select(house => house.ToXmlElement())
			)
		};

		return Element(nameof(ResidentialArea), children);
	}

	private static IXmlContainer ToXmlElement(this House house)
	{
		var children = new List<IXmlNode> {
			Attribute("type", house.Type),
			Element("Address",
				Element("street", Text(house.StreetName)),
				Element("number", Text(house.HouseNumber.ToString(CultureInfo.InvariantCulture))),
				Element("city", Text(house.ZipCode)),
				Element("zipCode", Text(house.ZipCode)),
				Element("country", Text(house.Country)),
				Element("letterValue",
					CData("\r\n" +
						  $"{house.StreetName} {house.HouseNumber}, " +
						  $"{house.ZipCode} {house.City}" + "\r\n" +
						  house.Country
						  +  "\r\n")
				)
			),
			Element("Residents",
				house.Residents.Select(person => person.ToXmlElement())
			)
		};

		return Element(nameof(House), children);
	}

	private static IXmlContainer ToXmlElement(this Person person)
	{
		var details = string.IsNullOrEmpty(person.MiddleName)
			? new List<IXmlElement>
			{
				Element("firstName", Text(person.FirstName)),
				Element("lastName", Text(person.LastName))

			}
			: new List<IXmlElement>
			{
				Element("firstName", Text(person.FirstName)),
				Element("middleName", Text(person.MiddleName)),
				Element("lastName", Text(person.LastName))
			};

		details.AddRange(new List<IXmlElement>
		{
			Element("gender", Text(person.Gender.ToString().ToLowerInvariant())),
			Element("dob", Text(person.DateOfBirth.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)))
		});

		var children = new List<IXmlNode> {
			Element("fullName", Text(
				person.MiddleName is null
					? string.Join(" ", person.FirstName, person.LastName)
					: string.Join(" ", person.FirstName, person.MiddleName, person.LastName))),
			Element("Details",
				details
			)
		};

		return Element(nameof(Person), children);
	}
}