using FluentSerializer.Core.BenchmarkUtils.TestData;
using FluentSerializer.Json.DataNodes;
using System.Collections.Generic;
using System.Linq;
using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Benchmark.Data
{
    public static class TestDataExtensions
    {
        private static IJsonValue StringValue(string value) => Value($"\"{value}\"");
        public static IJsonObject ToJsonElement(this ResidentialArea residentialArea)
        {
            var properties = new List<IJsonProperty> {
                Property("type", StringValue(residentialArea.Type)),
                Property("name", StringValue(residentialArea.Name)),
                Property("houses", Array(
                    residentialArea.Houses.Select(house => house.ToJsonElement())
                ))
            };

            return Object(properties);
        }

        public static IJsonObject ToJsonElement(this House house)
        {
            var properties = new List<IJsonProperty> {
                Property("type", StringValue(house.Type)),
                Property("address",
                    Object(
                        Property("street", StringValue(house.StreetName)),
                        Property("number", Value(house.HouseNumber.ToString())),
                        Property("city", StringValue(house.ZipCode)),
                        Property("zipCode", StringValue(house.ZipCode)),
                        Property("country", StringValue(house.Country))
                    )
                ),
                Property("residents", Array(
                    house.Residents.Select(person => person.ToJsonElement())
                ))
            };

            return Object(properties);
        }

        public static IJsonObject ToJsonElement(this Person person)
        {
            var properties = new List<IJsonProperty> {
                Property("fullName", StringValue(
                    person.MiddleName is null 
                        ? string.Join(" ", person.FirstName, person.LastName) 
                        : string.Join(" ", person.FirstName, person.MiddleName, person.LastName))),

                Property("details",
                    Object(
                        Property("firstName", StringValue(person.FirstName)),
                        Property("middleName", string.IsNullOrEmpty(person.MiddleName) ? null : StringValue(person.MiddleName)),
                        Property("lastName", StringValue(person.LastName)),
                        Property("gender", StringValue(person.Gender.ToString().ToLowerInvariant())),
                        Property("dob", StringValue(person.DateOfBirth.ToString("yyyy/MM/dd")))
                    )
                )
            };

            return Object(properties);
        }
    }
}
