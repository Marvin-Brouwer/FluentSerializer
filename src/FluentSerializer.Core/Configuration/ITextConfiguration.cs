using System.Text;

namespace FluentSerializer.Core.Configuration;

public interface ITextConfiguration
{
	bool UseSystemBuilder { get; }
	Encoding Encoding { get; }
	string NewLine { get; }
	bool UseWriteArrayPool { get; }
}