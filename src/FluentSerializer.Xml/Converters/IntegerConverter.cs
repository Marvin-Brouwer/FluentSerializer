namespace FluentSerializer.Xml.Converters
{
    public sealed class IntegerConverter : PrimitiveConverter<int>
    {
        public IntegerConverter() : base(ConvertToString, ConvertToInt) { }

        private static int ConvertToInt(string value) => int.Parse(value);

        private static string ConvertToString(int value) => value.ToString();
    }
}