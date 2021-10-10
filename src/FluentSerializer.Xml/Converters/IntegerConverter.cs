using FluentSerializer.Xml.Converters.Base;

namespace FluentSerializer.Xml.Converters
{
    public sealed class IntegerConverter : PrimitiveConverter<int>
    {
        protected override int ConvertToDataType(string value) => int.Parse(value);
        protected override string ConvertToString(int value) => value.ToString();
    }
}