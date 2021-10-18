using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Json.DataNodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSerializer.Json.Profiling.Data
{
    public static class TestDataExtensions
    {
        public static IJsonContainer ToJsonElement(this ResidentialArea residentialArea)
        {
            var properties = new List<JsonProperty> {
                new JsonProperty("type", JsonValue.String(residentialArea.Type)),
                new JsonProperty("name", JsonValue.String(residentialArea.Name)),
                new JsonProperty("houses", new JsonArray(
                    residentialArea.Houses.Select(house => house.ToJsonElement())
                ))
            };

            return new JsonObject(properties);
        }

        public static IJsonContainer ToJsonElement(this House house)
        {
            var properties = new List<JsonProperty> {
                new JsonProperty("type", JsonValue.String(house.Type)),
                new JsonProperty("address",
                    new JsonObject(
                        new JsonProperty("street", JsonValue.String(house.StreetName)),
                        new JsonProperty("number", new JsonValue(house.HouseNumber.ToString()))
                    )
                ),
                new JsonProperty("residents", new JsonArray(
                    house.Residents.Select(person => person.ToJsonElement())
                ))
            };

            return new JsonObject(properties);
        }

        public static IJsonContainer ToJsonElement(this Person person)
        {
            var properties = new List<JsonProperty> {
                new JsonProperty("fullName", JsonValue.String(
                    person.MiddleName is null 
                        ? string.Join(" ", person.FirstName, person.LastName) 
                        : string.Join(" ", person.FirstName, person.MiddleName, person.LastName))),
                new JsonProperty("details",
                    new JsonObject(
                        new JsonProperty("firstName", JsonValue.String(person.FirstName)),
                        new JsonProperty("middleName", person.MiddleName is null ? null : JsonValue.String(person.MiddleName)),
                        new JsonProperty("lastName", JsonValue.String(person.LastName)),
                        new JsonProperty("gender", JsonValue.String(person.Gender.ToString().ToLowerInvariant())),
                        new JsonProperty("dob", JsonValue.String(person.DateOfBirth.ToString("yyyy/MM/dd")))
                    )
                )
            };

            return new JsonObject(properties);
        }
    }
}
