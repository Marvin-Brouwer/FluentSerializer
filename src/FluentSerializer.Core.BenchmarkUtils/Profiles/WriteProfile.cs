using System;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.TestUtils.Helpers;

namespace FluentSerializer.Core.BenchmarkUtils.Profiles
{
	[MemoryDiagnoser]
    public abstract class WriteProfile
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

        public void Write(IDataNode value)
        {
            value.WriteTo(TestStringBuilderPool.StringBuilderPool, _streamWriter!, true);

            _streamWriter!.Flush();
            _ = Encoding.UTF8.GetString(_writeStream!.ToArray());
        }
    }
}
