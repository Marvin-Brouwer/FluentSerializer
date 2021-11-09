namespace FluentSerializer.Xml.DataNodes
{
    internal readonly struct XmlConstants
    {
        internal static readonly char TagStartCharacter = '<';
        internal static readonly char TagEndCharacter = '>';
        internal static readonly char TagTerminationCharacter = '/';

        internal static readonly char PropertyAssignmentCharacter = '=';
        internal static readonly char PropertyWrapCharacter = '"';

        internal const string CommentStart = "<!--";
        internal const string CommentEnd = "-->";

        internal const string CharacterDataStart = "<![CDATA[";
        internal const string CharacterDataEnd = "]]>";
    }
}
