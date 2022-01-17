using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Constants;
using System.Text;

namespace FluentSerializer.Core.TestUtils.Helpers;

internal readonly struct TestStringBuilderConfiguration : ITextConfiguration
{
	internal static TestStringBuilderConfiguration Default = new();
	internal static TestStringBuilderConfiguration NoArrayPool = new(false);

	private TestStringBuilderConfiguration(in bool useWriteArrayPool)
	{
		UseWriteArrayPool = useWriteArrayPool;
	}

	public int StringBuilderInitialCapacity => 100;
	public int StringBuilderMaximumRetainedCapacity => 4096;

	public Encoding Encoding => Encoding.UTF8;
	public string NewLine => LineEndings.LineFeed;
	public bool UseWriteArrayPool { get; }
}
