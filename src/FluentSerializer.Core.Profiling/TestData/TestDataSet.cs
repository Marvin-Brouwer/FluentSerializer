using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSerializer.Core.Profiling.TestData
{
    public readonly struct TestDataSet
    {
        private const int BogusSeed = 98123600;
        public readonly List<ResidentialArea> TestData;

        public TestDataSet(int setSize)
        {
#if(DEBUG)
            setSize = 100;
#endif
            Console.WriteLine($"Generating {setSize} bogus objects");
            TestData = BogusConfiguration.Generate(BogusSeed, setSize).ToList();

        }
    }
}
