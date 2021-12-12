using System;
using System.Text;

public interface ITextWriter
{
	Encoding Encoding { get; }

	ITextWriter Append(in char value);
	ITextWriter Append(in char character, in int repeat);
	ITextWriter Append(in string? value);
	ITextWriter Append(in ReadOnlySpan<char> value);
	ITextWriter AppendLineEnding();
	ITextWriter Clear();
	string ToString();

	Span<byte> AsSpan();
	Memory<byte> AsMemory();
}