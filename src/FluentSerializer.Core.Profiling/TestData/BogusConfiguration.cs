using Bogus;
using System.Collections.Generic;

namespace FluentSerializer.Core.Profiling.TestData
{
    public sealed class BogusConfiguration
    {
        private static readonly List<string> _areaTypes = new() { "city", "town", "village" };
        private static readonly List<string> _houseTypes = new() { "apartment", "detached", "condominium", "ranch" };
        public static List<ResidentialArea> Generate(int seed, int amount)
        {
            var personFaker = new Faker<Person>()
                .UseSeed(seed)
                .RuleFor(person => person.Gender, (f) => f.PickRandom<Bogus.DataSets.Name.Gender>())
                .RuleFor(person => person.FirstName, (f, p) => f.Name.FirstName(p.Gender))
                .RuleFor(person => person.MiddleName, f =>  f.Random
                    .WeightedRandom(new[] { true, false }, new [] { .7f, .3f }) ? null : f.Name.LastName())
                .RuleFor(person => person.LastName, (f, p) => f.Name.LastName(p.Gender))
                .RuleFor(person => person.DateOfBirth, f => f.Date.Past(f.Random.Number(18, 90)));

            var houseFaker = new Faker<House>()
                .UseSeed(seed)
                .RuleFor(house => house.Type, f => f.PickRandom(_houseTypes))
                .RuleFor(house => house.StreetName, f => f.Address.StreetName())
                .RuleFor(house => house.HouseNumber, f => f.Random.Number(min: 1))
                .RuleFor(house => house.ZipCode, f => f.Address.ZipCode())
                .RuleFor(house => house.Country, f => f.Address.Country())
                .RuleFor(house => house.Residents, f => personFaker.Generate(f.Random.Number(0, 5)));

            var residentialFaker = new Faker<ResidentialArea>()
                .UseSeed(seed)
                .RuleFor(residentialArea => residentialArea.Type, f => f.PickRandom(_areaTypes))
                .RuleFor(residentialArea => residentialArea.Name, f => f.Address.City())
                .RuleFor(residentialArea => residentialArea.Houses, (f, c) => {
                    var houses = houseFaker.Generate(f.Random.Number(0, 20));
                    houses.ForEach(house => house.City = c.Name);
                    return houses;
                });

            return residentialFaker
                .Generate(amount);
        }
    }
}
