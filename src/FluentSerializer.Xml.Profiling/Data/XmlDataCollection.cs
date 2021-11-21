using FluentSerializer.Core.Profiling.TestData;
using FluentSerializer.Xml.DataNodes;
using System.Collections.Generic;
using System.Linq;

using static FluentSerializer.Xml.XmlBuilder;

namespace FluentSerializer.Xml.Profiling.Data
{
    public sealed class XmlDataCollection : DataCollectionFactory<IXmlElement>
    {
#if (RELEASE)
        /// <summary>
        /// Override the amount of in-memory items because the XML serialzer seems to struggle with this.
        /// Anything around 25000 and up causes <see cref="System.OutOfMemoryException"/>s.
        /// </summary>
        protected override int DataItemCount => 22000;
#endif

        public static XmlDataCollection Default = new();

        protected override string GetStringFileName(string name) => $"{nameof(XmlDataCollection)}-{name}.xml";
        protected override IXmlElement ConvertToData(List<ResidentialArea> residentialAreas) =>
            Element("Data", residentialAreas.Select(area => area.ToXmlElement()));

    }
}
