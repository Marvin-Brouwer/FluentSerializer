using System.Text;

namespace FluentSerializer.Core.Configuration;

public interface ITextConfiguration
{
	Encoding Encoding { get; }
	string NewLine { get; }
}