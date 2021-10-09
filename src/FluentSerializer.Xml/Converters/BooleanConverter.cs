namespace FluentSerializer.Xml.Converters
{
    public sealed class BooleanConverter : PrimitiveConverter<bool>
    {
        protected override bool ConvertToDataType(string value) => bool.Parse(value);
        protected override string ConvertToString(bool value) => value.ToString().ToLower();
    }
}