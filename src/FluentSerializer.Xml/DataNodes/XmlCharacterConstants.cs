namespace FluentSerializer.Xml.DataNodes;

/// <summary>
/// Constants class containing all the characters used for parsing and writing JSON.
/// </summary>
internal readonly struct XmlCharacterConstants
{
	internal static readonly char TagStartCharacter = '<';
	internal static readonly char TagEndCharacter = '>';
	internal static readonly char TagTerminationCharacter = '/';

	internal static readonly char PropertyAssignmentCharacter = '=';
	internal static readonly char PropertyWrapCharacter = '"';

	internal const string DeclarationStart = "<?";
	internal const string DeclarationEnd = "?>";

	internal const string CommentStart = "<!--";
	internal const string CommentEnd = "-->";

	internal const string CharacterDataStart = "<![CDATA[";
	internal const string CharacterDataEnd = "]]>";
}