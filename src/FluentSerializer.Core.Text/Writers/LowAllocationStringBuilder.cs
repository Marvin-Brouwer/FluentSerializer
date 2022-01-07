using System;
using System.Buffers;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;

namespace FluentSerializer.Core.Text.Writers;

/// <summary>
/// Low allocation string builder, this String builder is highly custom to the purpose of writing out serialized data and is missing
/// some methods like formatted Appends because we don't need these.
/// </summary>
/// <remarks>
/// The implementation of this class is loosely based on `StringFast` <br />
/// Reddit: <see href="https://www.reddit.com/r/Unity3D/comments/3zz62z/alternative_to_stringbuilder_without_memory/">
/// Alternative to StringBuilder, without memory allocation and faster
/// </see><br />
/// Pastebin: <seealso href="https://pastebin.com/HqAw2pTG">
/// StringFast - optimized C# string class</seealso>  <br />
/// Author: Nicolas Gadenne contact@gaddygames.com
/// <br />
///  <br />
/// Even though this doesn't resemble the original file anymore, it helped us a lot and we think it's fair to give credit.
/// </remarks>
public sealed class LowAllocationStringBuilder : ITextWriter
{
	private const int DefaultChunkSize = 65536;
	public ITextConfiguration TextConfiguration { get; }

	private readonly ArrayPool<char>? _arrayPool;
	private string? _generatedStringValue;

	#region StringMutation
	private char[] _memoryBuffer;
	private readonly int _chunkSize;
	private int _currentBufferPosition;
	private int _bufferCapacity;
	#endregion

	public LowAllocationStringBuilder(in ITextConfiguration textConfiguration, in int chunkSize = DefaultChunkSize)
	{
		Guard.Against.NegativeOrZero(chunkSize, nameof(chunkSize));
		
		_chunkSize = chunkSize;
		_bufferCapacity = chunkSize;
		_generatedStringValue = null;
		_currentBufferPosition = 0;
		_bufferCapacity = 0;

		TextConfiguration = textConfiguration;
		_arrayPool = textConfiguration.UseWriteArrayPool ? ArrayPool<char>.Shared : null;
		_memoryBuffer = InitializeMemoryBuffer(_bufferCapacity);
		_generatedStringValue = null;
	}

	private char[] InitializeMemoryBuffer(in int size)
	{
		return _arrayPool?.Rent(size) ?? new char[size];
	}
	private void ReleaseMemoryBuffer(in char[] buffer)
	{
		_arrayPool?.Return(buffer, true);
	}

	public override string ToString() => _generatedStringValue ??= new string(_memoryBuffer, 0, _currentBufferPosition);

	public ITextWriter Clear()
	{
		_currentBufferPosition = 0;
		_generatedStringValue = null;
		ReleaseMemoryBuffer(_memoryBuffer);
		_memoryBuffer = InitializeMemoryBuffer(_bufferCapacity = _chunkSize);

		return this;
	}

	public ITextWriter AppendLineEnding() => Append(TextConfiguration.NewLine);

	#region AppendStringValue
	public ITextWriter Append(in string? value)
	{
		if (value is null) return this;

		ReallocateInternalBuffer(value.Length);
		CopyStringValue(value);

		return this;
	}

	public ITextWriter Append(in ReadOnlySpan<char> value)
	{
		if (value.IsEmpty) return this;

		ReallocateInternalBuffer(value.Length);
		CopyStringValue(value.ToString());

		return this;
	}

	private void CopyStringValue(string value)
	{
		int stringLength = value.Length;
		value.CopyTo(0, _memoryBuffer, _currentBufferPosition, stringLength);

		_currentBufferPosition += stringLength;
	}
	#endregion

	#region AppendCharacterValue
	public ITextWriter Append(in char value)
	{
		if (value == (char)0) return this;

		ReallocateInternalBuffer(1);
		CopyCharacterValue(value);

		return this;
	}
	public ITextWriter Append(in char value, in int repeat)
	{
		if (value == (char)0) return this;

		ReallocateInternalBuffer(repeat);
		CopyCharacterValues(value, repeat);

		return this;
	}

	private void CopyCharacterValue(char value)
	{
		_memoryBuffer[_currentBufferPosition] = value;
		_currentBufferPosition++;
	}

	private void CopyCharacterValues(char value, int repeat)
	{
		for (var i = 0; i < repeat; i++) CopyCharacterValue(value);
	}
	#endregion

	private void ReallocateInternalBuffer(in int amountOfCharactersAdded)
	{
		if (_currentBufferPosition + amountOfCharactersAdded <= _bufferCapacity) return;

		_bufferCapacity = Math.Max(_bufferCapacity + amountOfCharactersAdded, _bufferCapacity * 2);

		var newChars = InitializeMemoryBuffer(_bufferCapacity);
		_memoryBuffer.CopyTo(newChars, 0);
		ReleaseMemoryBuffer(_memoryBuffer);
		_memoryBuffer = newChars;
	}

	public Span<byte> AsSpan() => TextConfiguration.Encoding
		.GetBytes(_memoryBuffer)
		.AsSpan(0, _currentBufferPosition);

	public Memory<byte> AsMemory() => TextConfiguration.Encoding
		.GetBytes(_memoryBuffer)
		.AsMemory(0, _currentBufferPosition);

}