using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using FluentSerializer.Core.BenchmarkUtils.TestData;

namespace FluentSerializer.Core.BenchmarkUtils.Profiles
{
	[MemoryDiagnoser]
    public abstract class ReadProfile
    {
        private Stream? _textStream;
        private StreamReader? _reader;

        protected TestCase<Stream> CaseValue;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _textStream = CaseValue.GetData();
            _reader = new StreamReader(_textStream);
        }

        [IterationSetup]
        public void IterationSetup()
        {
            _textStream!.Seek(0, SeekOrigin.Begin);
			GC.Collect(0, GCCollectionMode.Forced, true);
			GC.Collect(1, GCCollectionMode.Forced, true);
			GC.Collect(2, GCCollectionMode.Forced, true);
			GC.WaitForPendingFinalizers();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _textStream?.Dispose();
            _reader?.Dispose();
            _textStream = null;
            _reader = null;
        }

        protected StreamReader CaseReader => _reader ?? StreamReader.Null;
    }
}
