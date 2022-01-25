using Ardalis.GuardClauses;
using System;

namespace FluentSerializer.Core.Text.Readers
{
	public struct StringTokenReader : ITokenReader
	{
		private readonly string _text;

		public StringTokenReader(in string text)
		{
			_text = text;
		}

		public bool CanAdvance(in int amount = 1)
		{
			Guard.Against.NegativeOrZero(amount, nameof(amount));

			return (Offset + amount) <= _text.Length;
		}

		public char CharacterAtOffset => _text[Offset];

		public int Offset { get; private set; } = 0;

		public void Advance(in int amount = 1)
		{
			Guard.Against.Negative(amount, nameof(amount));
			if (amount == 0) return;

			if (Offset + amount > _text.Length) throw new IndexOutOfRangeException(
				$"The current offset ({Offset}) plus {amount} cannot fit a buffer of {_text.Length} lenght");
			Offset += amount;
		}

		public bool HasCharacterAtOffset(in char character)
		{
			return CharacterAtOffset.Equals(character);
		}

		public bool HasStringAtOffset(in ReadOnlySpan<char> characters)
		{
			return _text[Offset..(Offset + characters.Length)]?.AsSpan()
				.Contains(characters, StringComparison.OrdinalIgnoreCase) ?? false;
		}

		public bool HasWhitespaceAtOffset()
		{
			return char.IsWhiteSpace(CharacterAtOffset);
		}

		public ReadOnlySpan<char> ReadAbsolute(Range range)
		{
			return _text[range];
		}
	}
}
