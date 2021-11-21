using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using FluentSerializer.Core.Tests.Helpers;
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

            XmlDataCollection.Default.GenerateObjectData();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            StreamWriter.Dispose();
            WriteStream.Dispose();
            StreamWriter = null;
            WriteStream = null;
        }


        [Benchmark, BenchmarkCategory("WriteTo")]
        public void XmlToString()
        {
            var value = XmlDataCollection.Default.ObjectTestData;

            value.WriteTo(TestStringBuilderPool.StringFastPool, true);
            StreamWriter.Flush();
            _ = Encoding.UTF8.GetString(WriteStream.ToArray());
        }
    }
}
