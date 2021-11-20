namespace FluentSerializer.Xml.DataNodes
{
    /// <summary>
    /// A representation of an XML document <br/>
    /// <see href="https://www.w3.org/TR/xml/#sec-documents" /> <br/><br/>
    /// </summary>
    public interface IXmlDocument : IXmlContainer<IXmlElement>
    {
        /// <summary>
        /// The root element containing this documents XML elements
        /// </summary>
        IXmlElement? RootElement { get; }
    }
}