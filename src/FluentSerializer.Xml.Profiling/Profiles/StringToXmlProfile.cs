using System;
using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Xml.Profiling.Data;

namespace FluentSerializer.Xml.Profiling.Profiles
{
    [MemoryDiagnoser]
    [InliningDiagnoser(true, true)]
    [TailCallDiagnoser]
    public class StringToXmlProfile
    {
        public IEnumerable<TestCase<Stream>> Values => XmlDataCollection.Default.StringTestData;

        [ParamsSource(nameof(Values))]
        public TestCase<Stream> Value;

        private Stream _textStream;
        private StreamReader _reader;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _textStream = Value.Data;
            _reader = new StreamReader(_textStream);
        }

        [IterationSetup]
        public void IterationSetup()
        {
            _textStream.Seek(0, SeekOrigin.Begin);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _textStream.Dispose();
            _reader.Dispose();
            _textStream = null;
            _reader = null;
        }

        [Benchmark, BenchmarkCategory("Parse")]
        public void StringToXml()
        {
            XmlParser.Parse(_reader.ReadToEnd());
        }
    }
}