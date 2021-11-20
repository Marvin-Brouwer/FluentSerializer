namespace FluentSerializer.Json.DataNodes
{
    /// <summary>
    /// Constants class containing all the characters used for parsing and writing JSON.
    /// </summary>
    internal readonly struct JsonCharacterConstants
    {
        internal const char ObjectStartCharacter = '{';
        internal const char ObjectEndCharacter = '}';

        internal const char ArrayStartCharacter = '[';
        internal const char ArrayEndCharacter = ']';

        internal const char PropertyWrapCharacter = '"';
        internal const char PropertyAssignmentCharacter = ':';

        internal const char DividerCharacter = ',';

        internal const string NullValue = "null";
        internal const string SingleLineCommentMarker = "//";
        internal const string MultiLineCommentStart = "/*";
        internal const string MultiLineCommentEnd = "*/";

        internal const char LineReturnCharacter = '\r';
        internal const char NewLineCharacter = '\n';
    }
}
