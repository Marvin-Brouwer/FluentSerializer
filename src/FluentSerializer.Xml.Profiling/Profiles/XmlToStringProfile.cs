using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Profiling.Data;

namespace FluentSerializer.Xml.Profiling.Profiles
{
    [MemoryDiagnoser]
    [InliningDiagnoser(true, true)]
    [TailCallDiagnoser]
    public class XmlToStringProfile
    {
        private MemoryStream WriteStream { get; set; }
        private StreamWriter StreamWriter { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            WriteStream = new MemoryStream();
            StreamWriter = new StreamWriter(WriteStream);
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            StreamWriter.Dispose();
            WriteStream.Dispose();
            StreamWriter = null;
            WriteStream = null;
        }

        public static IEnumerable<DataContainer<IXmlElement>> Inputs => XmlDataSet.XmlValues;

        [ParamsSource(nameof(Inputs))]
        public DataContainer<IXmlElement> Input { get; set; }

        [Benchmark, BenchmarkCategory("WriteTo")]
        public void WriteTo()
        {
            var result = Input.Value.WriteTo(XmlDataSet.StringFastPool, true);
            _ = result;
        }
    }
}
