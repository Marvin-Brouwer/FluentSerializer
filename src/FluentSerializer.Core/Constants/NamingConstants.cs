namespace FluentSerializer.Core.Constants;

internal readonly struct NamingConstants
{
	/// <summary>
	/// The character used in C# type names to indicate a generic type definition.
	/// </summary>
	internal const char GenericTypeMarker = '`';

	/// <summary>
	/// Pattern for all characters allowed in common naming conventions
	/// </summary>
	/// <remarks>
	/// Currently this is constrained by XML not allowing for ':', which is technically valid in JSON names.
	/// We might end up changing this in the future and validating an additional ':' on the XML side.
	/// However, if we ever support YAML this als problematic.
	/// </remarks>
	internal static readonly string ValidNamePattern =
		@"^[" +
			@"\w" + 
			SpecialCharacters.Underscore + 
			@"\" + SpecialCharacters.Minus + 
			SpecialCharacters.Plus +
		@"]*$";

	/// <summary>
	/// Special characters that need to be converted when certain namingstrategies are applied.
	/// </summary>
	internal readonly struct SpecialCharacters
	{
		internal const char Underscore = '_';
		internal const char Minus = '-';
		internal const char Plus = '+';
	}
}
