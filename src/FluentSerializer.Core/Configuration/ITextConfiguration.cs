using System.Text;

namespace FluentSerializer.Core.Configuration;

/// <summary>
/// Properties used to configure how the generating of text behaves.
/// </summary>
public interface ITextConfiguration
{
	/// <summary>
	/// Configure the initial capacity used for each <see cref="System.Text.StringBuilder"/> created in the Object pool
	/// </summary>
	int StringBuilderInitialCapacity { get; }
	/// <summary>
	/// Configure the maximum capacity for each <see cref="System.Text.StringBuilder"/> when returning to the Object pool
	/// </summary>
	int StringBuilderMaximumRetainedCapacity { get; }

	/// <summary>
	/// Encoding used when writing text to stream/disk/etc.
	/// </summary>
	Encoding Encoding { get; }

	/// <summary>
	/// Character used for newlines in text generation, see: <see cref="Constants.LineEndings"/>
	/// </summary>
	string NewLine { get; }
}