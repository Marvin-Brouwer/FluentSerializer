using System.Text;

namespace FluentSerializer.Core.Configuration;

public interface ITextConfiguration
{
	int StringBuilderInitialCapacity { get; }
	int StringBuilderMaximumRetainedCapacity { get; }
	
	Encoding Encoding { get; }
	string NewLine { get; }
	bool UseWriteArrayPool { get; }
}