using DEiXTo.Models;
using System.Xml;

namespace DEiXTo.Services
{
    public interface IExtractionPatternMapper
    {
        ExtractionPattern Map(XmlReader reader);
    }
}
