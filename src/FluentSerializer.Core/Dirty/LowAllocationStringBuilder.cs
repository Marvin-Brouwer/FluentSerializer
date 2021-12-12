using Ardalis.GuardClauses;
using System;
using System.Buffers;
using System.Text;

///<summary>
/// Mutable String class, optimized for speed and memory allocations while retrieving the final result as a string.
/// Similar use than StringFast, but avoid a lot of allocations done by StringFast (conversion of int and float to string, frequent capacity change, etc.)
/// Author: Nicolas Gadenne contact@gaddygames.com
///</summary>
public sealed class LowAllocationStringBuilder : ITextWriter
{
	private const int DefaultChunkSize = 65536;

	private readonly ArrayPool<char> _arrayPool;
	private string? _generatedStringValue;

	#region WriterSettings
	public Encoding Encoding { get; }
	private readonly string _newLine;
	#endregion

	#region StringMutation
	private char[] _memoryBuffer;
	private readonly int _chunkSize;
	private int _currentBufferPosition;
	private int _bufferCapacity;
	#endregion

	public LowAllocationStringBuilder(in Encoding encoding, in string newLine, in ArrayPool<char> arrayPool, in int chunkSize = DefaultChunkSize)
	{
		Guard.Against.NegativeOrZero(chunkSize, nameof(chunkSize));

		_chunkSize = chunkSize;
		_bufferCapacity = chunkSize;
		_generatedStringValue = null;
		_currentBufferPosition = 0;
		_bufferCapacity = 0;

		Encoding = encoding;
		_newLine = newLine;
		_arrayPool = arrayPool;
		_memoryBuffer = arrayPool.Rent(_bufferCapacity);
		_generatedStringValue = null;
	}

	public override string ToString() => _generatedStringValue ??= new string(_memoryBuffer, 0, _currentBufferPosition);

	public ITextWriter Clear()
	{
		_currentBufferPosition = 0;
		_generatedStringValue = null;
		_arrayPool.Return(_memoryBuffer, true);
		_memoryBuffer = _arrayPool.Rent(_bufferCapacity = _chunkSize);

		return this;
	}

	public ITextWriter AppendLineEnding() => Append(_newLine);

	#region AppendStringValue
	public ITextWriter Append(in string? value)
	{
		if (value is null) return this;

		ReallocateIFN(value.Length);
		CopyStringValue(value);

		return this;
	}

	public ITextWriter Append(in ReadOnlySpan<char> value)
	{
		if (value.IsEmpty) return this;

		ReallocateIFN(value.Length);
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

		ReallocateIFN(1);
		CopyCharacterValue(value);

		return this;
	}
	public ITextWriter Append(in char value, in int repeat)
	{
		if (value == (char)0) return this;

		ReallocateIFN(repeat);
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

	private void ReallocateIFN(in int amountOfCharactersAdded)
	{
		if (_currentBufferPosition + amountOfCharactersAdded <= _bufferCapacity) return;

		_bufferCapacity = Math.Max(_bufferCapacity + amountOfCharactersAdded, _bufferCapacity * 2);

		var newChars = _arrayPool.Rent(_bufferCapacity);
		_memoryBuffer.CopyTo(newChars, 0);
		_arrayPool.Return(_memoryBuffer, true);
		_memoryBuffer = newChars;
	}

	public Span<byte> AsSpan() => Encoding.GetBytes(_memoryBuffer).AsSpan(0, _currentBufferPosition);

	public Memory<byte> AsMemory() => Encoding.GetBytes(_memoryBuffer).AsMemory(0, _currentBufferPosition);

}