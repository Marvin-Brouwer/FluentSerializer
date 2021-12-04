using BenchmarkDotNet.Attributes;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.TestUtils.Helpers;
using System;
using System.IO;
using System.Text;

namespace FluentSerializer.Core.Profiling.Profilers
{
	[MemoryDiagnoser]
    public abstract class ReadProfile
    {
        private MemoryStream? _writeStream;
        private StreamWriter? _streamWriter;

        [GlobalSetup]
        public virtual void GlobalSetup()
        {
            _writeStream = new MemoryStream();
            _streamWriter = new StreamWriter(_writeStream);
        }

        [IterationSetup]
        public virtual void IterationSetup()
        {
            _writeStream!.Seek(0, SeekOrigin.Begin);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        [GlobalCleanup]
        public virtual void GlobalCleanup()
        {
            _streamWriter?.Dispose();
            _writeStream?.Dispose();
            _streamWriter = null;
            _writeStream = null;
        }

        public virtual void Read(IDataNode value)
        {
            value.WriteTo(TestStringBuilderPool.StringFastPool, true);
            _streamWriter!.Flush();
            _ = Encoding.UTF8.GetString(_writeStream!.ToArray());
        }
    }
}
