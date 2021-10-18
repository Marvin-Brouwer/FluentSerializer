using System.Collections.Generic;

namespace FluentSerializer.Core.Profiling.TestData
{
    public sealed class House
    {
        public string Type { get; set; }
        public string StreetName { get; set; }
        public int HouseNumber { get; set; }

        public List<Person> Residents { get; set; }
    }
}
