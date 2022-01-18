using System;

namespace FluentSerializer.Core.Text
{
	public interface ITextReader
	{
		void Advance(in int amount = 1);
		bool CanAdvance(in int amount = 1); 

		string ReadAbsolute(Range range);

		char CharacterAtOffset { get; }
		int Offset { get; }
		bool HasCharacterAtOffset(in char character);
		bool HasStringAtOffset(in string characters);
		bool HasWhitespaceAtOffset();
	}
}
