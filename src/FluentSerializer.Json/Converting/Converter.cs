namespace FluentSerializer.Json.Converting
{
    public readonly struct Converter
    {
        public static IUseJsonConverters For { get; } = new UseJsonConverters();
    }
}
