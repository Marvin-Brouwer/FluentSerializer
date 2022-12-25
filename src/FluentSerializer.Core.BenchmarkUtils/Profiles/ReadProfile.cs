using BenchmarkDotNet.Attributes;

using FluentSerializer.Core.BenchmarkUtils.TestData;

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace FluentSerializer.Core.BenchmarkUtils.Profiles;

#pragma warning disable S3881 // "IDisposable" should be implemented correctly

[MemoryDiagnoser]
public abstract class ReadProfile : IDisposable
{
	private Stream? _textStream;
	private StreamReader? _reader;

	protected TestCase<Stream> CaseValue { get; set; }

	[GlobalSetup]
	public void GlobalSetup()
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

		_textStream = CaseValue.GetData();
		_reader = new StreamReader(_textStream);
	}

	[IterationSetup]
	public void IterationSetup()
	{
		_textStream!.Seek(0, SeekOrigin.Begin);
#pragma warning disable S1215 // "GC.Collect" should not be called
		GC.Collect(0, GCCollectionMode.Forced, true);
		GC.Collect(1, GCCollectionMode.Forced, true);
		GC.Collect(2, GCCollectionMode.Forced, true);
#pragma warning restore S1215 // "GC.Collect" should not be called
		GC.WaitForPendingFinalizers();
	}

	protected StreamReader CaseReader => _reader ?? StreamReader.Null;

	[GlobalCleanup]
	public void Dispose()
	{
		GC.SuppressFinalize(this);

		_textStream?.Dispose();
		_reader?.Dispose();
		_textStream = null;
		_reader = null;
	}
}