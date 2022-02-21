using System;
using FluentSerializer.Core.Configuration;

namespace FluentSerializer.Core.Text;

/// <summary>
/// Internal implementation of a string appending class<br />
/// The reason we're using a custom implementation: <br />
/// - Having a plug-able implementation<br />
/// - Having control over the newline characters<br />
/// - Having a clear definition of which methods to use when building any serializer library
/// </summary>
public interface ITextWriter
{
	/// <inheritdoc cref="ITextConfiguration"/>
	public ITextConfiguration TextConfiguration { get; }

	/// <inheritdoc cref="System.Text.StringBuilder.Append(char)"/>
	ITextWriter Append(in char value);
	/// <inheritdoc cref="System.Text.StringBuilder.Append(int)"/>
	ITextWriter Append(in char value, in int repeat);
	/// <inheritdoc cref="System.Text.StringBuilder.Append(string?)"/>
	ITextWriter Append(in string? value);
	/// <inheritdoc cref="System.Text.StringBuilder.Append(ReadOnlySpan{char})"/>
	ITextWriter Append(in ReadOnlySpan<char> value);
	/// <summary>
	/// Append a new line using <see cref="ITextConfiguration.NewLine"/>
	/// </summary>
	/// <returns></returns>
	ITextWriter AppendLineEnding();
	/// <inheritdoc cref="System.Text.StringBuilder.Clear"/>
	ITextWriter Clear();
	/// <inheritdoc cref="System.Text.StringBuilder.ToString()"/>
	string ToString();

	/// <summary>
	/// Return the internal buffer as a <see cref="Span{T}"/>
	/// </summary>
	Span<byte> AsSpan();
	/// <summary>
	/// Return the internal buffer as a <see cref="Memory{T}"/>
	/// </summary>
	Memory<byte> AsMemory();
}