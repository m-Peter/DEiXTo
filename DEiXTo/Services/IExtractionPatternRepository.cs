using DEiXTo.Models;

namespace DEiXTo.Services
{
    public interface IExtractionPatternRepository
    {
        ExtractionPattern Load();
        void Save(ExtractionPattern pattern);
    }
}
