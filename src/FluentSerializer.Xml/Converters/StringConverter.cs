namespace FluentSerializer.Xml.Converters
{
    public sealed class StringConverter : SimpleTypeConverter<string>
    {
        protected override string ConvertToDataType(string value) => value;
        protected override string ConvertToString(string value) => value;
    }
}