using System.Collections.Generic;

namespace FluentSerializer.Core.Profiling.TestData
{
    public sealed class ResidentialArea
    {
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<House> Houses { get; set; } = new();
    }
}
