using System;

namespace FluentSerializer.Core.Profiling.TestData
{
    public sealed class Person
    {
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public Bogus.DataSets.Name.Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
