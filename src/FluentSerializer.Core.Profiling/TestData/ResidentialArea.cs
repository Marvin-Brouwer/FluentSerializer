using System.Collections.Generic;

namespace FluentSerializer.Core.Profiling.TestData
{
    public sealed class ResidentialArea
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public List<House> Houses { get; set; }
    }
}
