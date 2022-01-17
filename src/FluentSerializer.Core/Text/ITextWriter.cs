using System;
using FluentSerializer.Core.Configuration;

namespace FluentSerializer.Core.Text;

public interface ITextWriter
{
	public ITextConfiguration TextConfiguration { get; }

	ITextWriter Append(in char value);
	ITextWriter Append(in char value, in int repeat);
	ITextWriter Append(in string? value);
	ITextWriter Append(in ReadOnlySpan<char> value);
	ITextWriter AppendLineEnding();
	ITextWriter Clear();
	string ToString();

	Span<byte> AsSpan();
	Memory<byte> AsMemory();
}