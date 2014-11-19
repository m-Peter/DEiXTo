using DEiXTo.Models;
using System.IO;

namespace DEiXTo.Services
{
    public interface IExtractionPatternRepository
    {
        ExtractionPattern Load(Stream stream);
        void Save(ExtractionPattern pattern, Stream stream);
    }
}
