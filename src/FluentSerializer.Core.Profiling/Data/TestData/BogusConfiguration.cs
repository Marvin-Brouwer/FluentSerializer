using Bogus;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentSerializer.Core.Profiling.Data.TestData
{
    public sealed class BogusConfiguration
    {
        private static readonly List<string> _areaTypes = new() { "city", "town", "village" };
        private static readonly List<string> _houseTypes = new() { "apartment", "detached", "condominium", "ranch" };
        public static List<ResidentialArea> Generate(int seed)
        {
            Randomizer.Seed = new Random(seed);

            var personFaker = new Faker<Person>();
            personFaker.RuleFor(person => person.Gender, (f) => f.PickRandom<Bogus.DataSets.Name.Gender>());
            personFaker.RuleFor(person => person.FirstName, (f, p) => f.Name.FirstName(p.Gender));
            personFaker.RuleFor(person => person.MiddleName, f => 
                f.Random.WeightedRandom(new[] { true, false }, new [] { .7f, .3f }) ? null : f.Name.LastName());
            personFaker.RuleFor(person => person.LastName, (f, p) => f.Name.LastName(p.Gender));
            personFaker.RuleFor(person => person.DateOfBirth, f => f.Date.Past(f.Random.Number(18, 90)));

            var houseFaker = new Faker<House>();
            houseFaker.RuleFor(house => house.Type, f => f.PickRandom(_houseTypes));
            houseFaker.RuleFor(house => house.StreetName, f => f.Address.StreetName());
            houseFaker.RuleFor(house => house.HouseNumber, f => f.Random.Number(min: 1));
            houseFaker.RuleFor(house => house.Residents, f => personFaker.Generate(f.Random.Number(0, 4)));

            var residentialFaker = new Faker<ResidentialArea>();
            residentialFaker.RuleFor(residentialArea => residentialArea.Type, f => f.PickRandom(_areaTypes));
            residentialFaker.RuleFor(residentialArea => residentialArea.Name, f => f.Address.City());
            residentialFaker.RuleFor(residentialArea => residentialArea.Houses, f => houseFaker.Generate(f.Random.Number(0, 500)));

            return residentialFaker.Generate(20);
        }
    }
}
