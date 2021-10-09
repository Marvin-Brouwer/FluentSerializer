using FluentSerializer.Core.Services;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Profiles
{
    public interface ICustomElementConverter : IConverter<XElement>
    {
    }
}
