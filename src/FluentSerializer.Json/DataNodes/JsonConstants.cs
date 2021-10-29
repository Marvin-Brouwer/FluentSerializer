namespace FluentSerializer.Json.DataNodes
{
    internal readonly struct JsonConstants
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
