using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Constants;
using System.Text;

namespace FluentSerializer.Core.TestUtils.Helpers
{
	internal readonly struct TestStringBuilderConfiguration : ITextConfiguration
	{
		internal static TestStringBuilderConfiguration Default = new();

		public Encoding Encoding => Encoding.UTF8;
		public string NewLine => LineEndings.LineFeed;
	}
}