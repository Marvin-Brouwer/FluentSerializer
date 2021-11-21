using System;

///<summary>
/// Mutable String class, optimized for speed and memory allocations while retrieving the final result as a string.
/// Similar use than StringFast, but avoid a lot of allocations done by StringFast (conversion of int and float to string, frequent capacity change, etc.)
/// Author: Nicolas Gadenne contact@gaddygames.com
///</summary>
public sealed class StringFast
{
	private readonly string _newLine = Environment.NewLine;

	///<summary>Immutable string. Generated at last moment, only if needed</summary>
	private string? _generatedStringValue = null;
    private readonly int _initialCapacity;

    ///<summary>Working mutable string</summary>
    private char[] _memoryBuffer;
    private int _currentBufferPosition = 0;
	private int _characterCapacity = 0;

	// ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

	public StringFast() : this(Environment.NewLine)
	{
	}

	public StringFast(in string newLine, in int initialCapacity = 32) : this(initialCapacity)
	{
		_newLine = newLine;
	}

	public StringFast(in int initialCapacity)
	{
		_initialCapacity = initialCapacity;
		_characterCapacity = initialCapacity;
		_memoryBuffer = new char[_characterCapacity];
		_generatedStringValue = null;
	}

    ///<summary>Return the string</summary>
    public override string ToString() => _generatedStringValue ??= new string(_memoryBuffer, 0, _currentBufferPosition);

    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    // Append methods, to build the string without allocation

    ///<summary>Reset the m_char array</summary>
    public StringFast Clear()
	{
		_currentBufferPosition = 0;
		_generatedStringValue = null; 
		_memoryBuffer = new char[_characterCapacity = _initialCapacity];

		return this;
	}

    public StringFast AppendLineEnding() => Append(_newLine);

    public StringFast Append(in char character, in int repeat)
	{
		for (var i = 0; i < repeat; i++) Append(character);
		return this;
	}

	///<summary>Append a string without memory allocation</summary>
	public StringFast Append(in string? value)
	{
		if (value is null) return this;
		ReallocateIFN(value.Length);

		int stringLength = value.Length;
		value.CopyTo(0, _memoryBuffer, _currentBufferPosition, stringLength);

		_currentBufferPosition += stringLength;

		return this;
	}

	///<summary>Append a character without memory allocation</summary>
	public StringFast Append(in char value)
	{
		if (value == (char)0) return this;

		ReallocateIFN(1);

		_memoryBuffer[_currentBufferPosition] = value;
		_currentBufferPosition ++;

		return this;
	}

	private void ReallocateIFN(in int amountOfCharactersAdded)
    {
		if (_currentBufferPosition + amountOfCharactersAdded <= _characterCapacity) return;

        _characterCapacity = Math.Max(_characterCapacity + amountOfCharactersAdded, _characterCapacity * 2);
        char[] newChars = new char[_characterCapacity];
        _memoryBuffer.CopyTo(newChars, 0);
        _memoryBuffer = newChars;
    }

}