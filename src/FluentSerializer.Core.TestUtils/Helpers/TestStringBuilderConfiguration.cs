using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Constants;

using System.Text;

namespace FluentSerializer.Core.TestUtils.Helpers;

internal readonly struct TestStringBuilderConfiguration : ITextConfiguration
{

#pragma warning disable CS0649
	internal static TestStringBuilderConfiguration Default;
#pragma warning restore CS0649

	public int StringBuilderInitialCapacity => 100;
	public int StringBuilderMaximumRetainedCapacity => 4096;

	public Encoding Encoding => Encoding.UTF8;
	public string NewLine => LineEndings.LineFeed;
}
