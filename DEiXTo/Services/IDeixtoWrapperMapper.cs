using DEiXTo.Models;
using System.Xml;

namespace DEiXTo.Services
{
    public interface IDeixtoWrapperMapper
    {
        DeixtoWrapper Map(XmlReader reader);
    }
}
