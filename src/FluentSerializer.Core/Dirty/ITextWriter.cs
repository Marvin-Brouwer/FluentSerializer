using System;

public interface ITextWriter
{
	StringFast Append(in char value);
	StringFast Append(in char character, in uint repeat);
	StringFast Append(in string? value);
	StringFast AppendLineEnding();
	StringFast Clear();
	string ToString();
	ReadOnlySpan<byte> GetBytes();
}