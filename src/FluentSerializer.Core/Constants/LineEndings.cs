namespace FluentSerializer.Core.Constants;

/// <summary>
/// Helper class containing constants for common line ending configurations
/// </summary>
public static class LineEndings
{
	/// <summary>
	/// The line feed (\n) character typically used in Unix based OS's
	/// </summary>
	public const string LineFeed = "\n";
	/// <summary>
	/// The carriage return character (\r) typically used in MacOs
	/// </summary>
	public const string CarriageReturn = "\r";
	/// <summary>
	/// A carriage return followed by a line feed (\r\n) typically used in Windows
	/// </summary>
	public const string ReturnLineFeed = CarriageReturn + LineFeed;

	/// <summary>
	/// The line ending configured for the current OS
	/// </summary>

	public static readonly string Environment = System.Environment.NewLine;
}