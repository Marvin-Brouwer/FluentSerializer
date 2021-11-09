namespace FluentSerializer.Xml.DataNodes
{
    public interface IXmlDocument : IXmlContainer<IXmlElement>
    {
        IXmlElement? RootElement { get; }
    }
}