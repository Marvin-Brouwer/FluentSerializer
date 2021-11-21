using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Json.DataNodes;
using Microsoft.Extensions.ObjectPool;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static FluentSerializer.Json.JsonBuilder;

namespace FluentSerializer.Json.Profiling.Data
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
        public static void ToStringRepresentation(this ResidentialArea residentialArea, StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("\tObject(");
            stringBuilder.AppendLine($"\t\tProperty(\"type\", Value(\"{WrapInQuotes(residentialArea.Type)}\")),");
            stringBuilder.AppendLine($"\t\tProperty(\"name\", Value(\"{WrapInQuotes(residentialArea.Name)}\")),");
            stringBuilder.AppendLine($"\t\tProperty(\"houses\", Array(");

            foreach(var house in residentialArea.Houses)
            {
                house.ToStringRepresentation(stringBuilder);
                if (!house.Equals(residentialArea.Houses[^1]))
                    stringBuilder.AppendLine(",");
            }

            stringBuilder.AppendLine("\t\t))");
            stringBuilder.AppendLine("\t)");
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
        public static void ToStringRepresentation(this House house, StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("\t\t\tObject(");
            stringBuilder.AppendLine($"\t\t\t\tProperty(\"type\", Value(\"{WrapInQuotes(house.Type)}\")),");
            stringBuilder.AppendLine("\t\t\t\tProperty(\"address\", Object(");
            stringBuilder.AppendLine($"\t\t\t\t\tProperty(\"street\", Value(\"{WrapInQuotes(house.StreetName)}\")),");
            stringBuilder.AppendLine($"\t\t\t\t\tProperty(\"number\", Value(\"{house.HouseNumber}\")),");
            stringBuilder.AppendLine($"\t\t\t\t\tProperty(\"city\", Value(\"{WrapInQuotes(house.ZipCode)}\")),");
            stringBuilder.AppendLine($"\t\t\t\t\tProperty(\"zipCode\", Value(\"{WrapInQuotes(house.ZipCode)}\")),");
            stringBuilder.AppendLine($"\t\t\t\t\tProperty(\"country\", Value(\"{WrapInQuotes(house.Country)}\"))");
            stringBuilder.AppendLine("\t\t\t\t)),");
            stringBuilder.AppendLine($"\t\t\t\tProperty(\"residents\", Array(");

            foreach (var resident in house.Residents)
            {
                resident.ToStringRepresentation(stringBuilder);
                if (!resident.Equals(house.Residents[^1]))
                    stringBuilder.AppendLine(",");
            }

            stringBuilder.AppendLine("\t\t\t))");
            stringBuilder.AppendLine("\t\t)");
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
                        Property("middleName", StringValue(person.MiddleName)),
                        Property("lastName", StringValue(person.LastName)),
                        Property("gender", StringValue(person.Gender.ToString().ToLowerInvariant())),
                        Property("dob", StringValue(person.DateOfBirth.ToString("yyyy/MM/dd")))
                    )
                )
            };

            return Object(properties);
        }
        public static void ToStringRepresentation(this Person person, StringBuilder stringBuilder)
        {
            var fullName = person.MiddleName is null
                ? string.Join(" ", person.FirstName, person.LastName)
                : string.Join(" ", person.FirstName, person.MiddleName, person.LastName);
            stringBuilder.AppendLine("\t\t\t\t\tObject(");
            stringBuilder.AppendLine($"\t\t\t\t\t\tProperty(\"fullName\", Value(\"{WrapInQuotes(fullName)}\")),");
            stringBuilder.AppendLine("\t\t\t\t\t\tProperty(\"details\", Object(");
            stringBuilder.AppendLine($"\t\t\t\t\t\t\tProperty(\"firstName\", Value(\"{WrapInQuotes(person.FirstName)}\")),");
            stringBuilder.AppendLine($"\t\t\t\t\t\t\tProperty(\"middleName\", Value(\"{(person.MiddleName is null ? "null" : WrapInQuotes(person.MiddleName))}\")),");
            stringBuilder.AppendLine($"\t\t\t\t\t\t\tProperty(\"lastName\", Value(\"{WrapInQuotes(person.LastName)}\")),");
            stringBuilder.AppendLine($"\t\t\t\t\t\t\tProperty(\"gender\", Value(\"{WrapInQuotes(person.Gender.ToString().ToLowerInvariant())}\")),");
            stringBuilder.AppendLine($"\t\t\t\t\t\t\tProperty(\"dob\", Value(\"{WrapInQuotes(person.DateOfBirth.ToString("yyyy/MM/dd"))}\"))");
            stringBuilder.AppendLine("\t\t\t\t\t\t))");
            stringBuilder.AppendLine("\t\t\t\t\t)");
        }

        private static string WrapInQuotes(string value) => $"\\\"{value}\\\"";
    }
}
