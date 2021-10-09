namespace FluentSerializer.Xml.Converters
{
    public sealed class BooleanConverter : PrimitiveConverter<bool>
    {
        public BooleanConverter() : base(ConvertToString, ConvertToBool) { }

        private static bool ConvertToBool(string value) => bool.Parse(value);

        private static string ConvertToString(bool value) => value.ToString().ToLower();
    }
}