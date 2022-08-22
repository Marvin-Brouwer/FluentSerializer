using BenchmarkDotNet.Attributes;

using FluentSerializer.Core.BenchmarkUtils.TestData;

using System;
using System.IO;

namespace FluentSerializer.Core.BenchmarkUtils.Profiles;

[MemoryDiagnoser]
public abstract class ReadProfile : IDisposable
{
	private Stream? _textStream;
	private StreamReader? _reader;

	protected TestCase<Stream> CaseValue { get; set; }

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