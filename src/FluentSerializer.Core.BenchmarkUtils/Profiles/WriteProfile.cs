using BenchmarkDotNet.Attributes;

using FluentSerializer.Core.BenchmarkUtils.TestData;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.TestUtils.Helpers;

using System;
using System.IO;
using System.Text;

namespace FluentSerializer.Core.BenchmarkUtils.Profiles;

#pragma warning disable S3881 // "IDisposable" should be implemented correctly

[MemoryDiagnoser]
public abstract class WriteProfile : IDisposable
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
#pragma warning disable S1215 // "GC.Collect" should not be called
		GC.Collect(0, GCCollectionMode.Forced, true);
		GC.Collect(1, GCCollectionMode.Forced, true);
		GC.Collect(2, GCCollectionMode.Forced, true);
#pragma warning restore S1215 // "GC.Collect" should not be called
		GC.WaitForPendingFinalizers();
	}

	public string Write(TestCase<IDataNode> testCase)
	{
		testCase.GetData().WriteTo(TestStringBuilderPool.Default, true);
		_streamWriter!.Flush();

		return Encoding.UTF8.GetString(_writeStream!.ToArray());
	}

	[GlobalCleanup]
	public void Dispose()
	{
		GC.SuppressFinalize(this);

		_streamWriter?.Dispose();
		_writeStream?.Dispose();
		_streamWriter = null;
		_writeStream = null;
	}
}