using System.Collections.Generic;
using System.Linq;

namespace FluentSerializer.Core.Profiling.TestData
{
    public readonly struct TestDataSet
    {
        private const int BogusSeed = 98123600;
        public static readonly List<ResidentialArea> TestData = BogusConfiguration.Generate(BogusSeed, SetSize).ToList();

#if (DEBUG)
        private const int SetSize = 100;
#else
        private const int SetSize = 10000;
#endif
    }
}
