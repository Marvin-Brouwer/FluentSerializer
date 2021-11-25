﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using FluentSerializer.Core.Profiling.TestData;
using System;
using System.IO;

namespace FluentSerializer.Core.Profiling.Profilers
{
    [MemoryDiagnoser]
    [InliningDiagnoser(true, true)]
    [TailCallDiagnoser]
    public abstract class WriteProfile
    {
        private Stream? _textStream;
        protected StreamReader? _reader;

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
            GC.Collect();
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
