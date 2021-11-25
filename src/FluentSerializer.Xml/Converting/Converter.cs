namespace FluentSerializer.Xml.Converting
{
    public readonly struct Converter
    {
        public static IUseXmlConverters For { get; } = new UseXmlConverters();
    }
}
