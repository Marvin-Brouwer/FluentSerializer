using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Core.Tests.Helpers;
using FluentSerializer.Xml.DataNodes;
using FluentSerializer.Xml.Profiling.Data;

namespace FluentSerializer.Xml.Profiling.Profiles
{
    [MemoryDiagnoser]
    [InliningDiagnoser(true, true)]
    [TailCallDiagnoser]
    public class XmlToStringProfile
    {
        public IEnumerable<TestCase<IXmlElement>> Values() => XmlDataCollection.Default.ObjectTestData;

        [ParamsSource(nameof(Values))]
        public TestCase<IXmlElement> Value;

        private MemoryStream _writeStream;
        private StreamWriter _streamWriter;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _writeStream = new MemoryStream();
            _streamWriter = new StreamWriter(_writeStream);
        }

        [IterationSetup]
        public void IterationSetup()
        {
            _writeStream.Seek(0, SeekOrigin.Begin);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _streamWriter.Dispose();
            _writeStream.Dispose();
            _streamWriter = null;
            _writeStream = null;
        }

        [Benchmark, BenchmarkCategory("WriteTo")]
        public void XmlToString()
        {
            Value.Data.WriteTo(TestStringBuilderPool.StringBuilderPool, _streamWriter, true);
            _streamWriter.Flush();
            _ = Encoding.UTF8.GetString(_writeStream.ToArray());
        }
    }
}
