using Bogus;
using System.Collections.Generic;

namespace FluentSerializer.Core.BenchmarkUtils.TestData
{
    public readonly struct BogusConfiguration
    {
        private static readonly List<string> AreaTypes = new() { "city", "town", "village" };
        private static readonly List<string> HouseTypes = new() { "apartment", "detached", "condominium", "ranch" };
        public static (List<ResidentialArea> data, long houseCount, long peopleCount) Generate(int seed, int amount)
        {
            long peopleCount = 0;
            long houseCount = 0;

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
                .RuleFor(house => house.Type, f => f.PickRandom(HouseTypes))
                .RuleFor(house => house.StreetName, f => f.Address.StreetName())
                .RuleFor(house => house.HouseNumber, f => f.Random.Number(min: 1, max: 409))
                .RuleFor(house => house.ZipCode, f => f.Address.ZipCode())
                .RuleFor(house => house.Country, f => f.Address.Country())
                .RuleFor(house => house.Residents, f =>
                {
                    var amount = f.Random.Number(1, 5);
                    peopleCount += amount;

                    return personFaker.Generate(amount);
                });

            var residentialFaker = new Faker<ResidentialArea>()
                .UseSeed(seed)
                .RuleFor(residentialArea => residentialArea.Type, f => f.PickRandom(AreaTypes))
                .RuleFor(residentialArea => residentialArea.Name, f => f.Address.City())
                .RuleFor(residentialArea => residentialArea.Houses, (f, c) => {

                    var amount = f.Random.Number(3, 20);
                    houseCount += amount;

                    var houses = houseFaker.Generate(amount);
                    houses.ForEach(house => house.City = c.Name);
                    return houses;
                });

            var data = residentialFaker.Generate(amount);
            return (data, houseCount, peopleCount);
        }
    }
}
