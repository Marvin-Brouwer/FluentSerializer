using System;

namespace FluentSerializer.Core.Profiling.TestData
{
    public sealed class Person
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public Bogus.DataSets.Name.Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
