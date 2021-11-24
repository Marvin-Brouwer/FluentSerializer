using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Core.Tests.Helpers;
using FluentSerializer.Json.DataNodes;
using FluentSerializer.Json.Profiling.Data;

namespace FluentSerializer.Json.Profiling.Profiles
{
    [MemoryDiagnoser]
    [InliningDiagnoser(true, true)]
    [TailCallDiagnoser]
    public class JsonToStringProfile
    {
        public IEnumerable<TestCase<IJsonObject>> Values() => JsonDataCollection.Default.ObjectTestData;

        [ParamsSource(nameof(Values))]
        public TestCase<IJsonObject> Value;

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
        public void JsonToString()
        {
            Value.Data.WriteTo(TestStringBuilderPool.StringBuilderPool, _streamWriter, true);
            _streamWriter.Flush();
            _ = Encoding.UTF8.GetString(_writeStream.ToArray());
        }
    }
}
