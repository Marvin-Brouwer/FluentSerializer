using System;

namespace FluentSerializer.Core.Text
{
	public interface ITokenReader
	{
		void Advance(in int amount = 1);
		bool CanAdvance(in int amount = 1); 

		ReadOnlySpan<char> ReadAbsolute(Range range);

		char CharacterAtOffset { get; }
		int Offset { get; }
		bool HasCharacterAtOffset(in char character);
		bool HasStringAtOffset(in ReadOnlySpan<char> characters);
		bool HasWhitespaceAtOffset();
	}
}
