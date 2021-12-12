using System;

public interface ITextWriter
{
	ITextWriter Append(in char value);
	ITextWriter Append(in char character, in uint repeat);
	ITextWriter Append(in string? value);
	ITextWriter AppendLineEnding();
	ITextWriter Clear();
	string ToString();
	ReadOnlySpan<byte> GetBytes();
}