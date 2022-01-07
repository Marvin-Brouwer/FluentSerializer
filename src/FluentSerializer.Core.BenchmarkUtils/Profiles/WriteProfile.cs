using System;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using FluentSerializer.Core.BenchmarkUtils.TestData;
using FluentSerializer.Core.DataNodes;
using FluentSerializer.Core.TestUtils.Helpers;

namespace FluentSerializer.Core.BenchmarkUtils.Profiles;

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
		GC.Collect(0, GCCollectionMode.Forced, true);
		GC.Collect(1, GCCollectionMode.Forced, true);
		GC.Collect(2, GCCollectionMode.Forced, true);
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

	public string Write(TestCase<IDataNode> testCase)
	{
		// We don't care about comparing the difference between these to atm, this only makes the benchmark run longer
		// whilst we already know the outcome.
		// Remark: Add a note about this when adding benchmarks to the readme or make it a run param.
		var stringBuilderPool = testCase.Count > 1000
			? TestStringBuilderPool.Default
			: TestStringBuilderPool.NoArrayPool;

		testCase.GetData().WriteTo(stringBuilderPool, true);
		_streamWriter!.Flush();

		return Encoding.UTF8.GetString(_writeStream!.ToArray());
	}
}