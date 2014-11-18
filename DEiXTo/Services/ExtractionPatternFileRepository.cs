using DEiXTo.Models;
using System.IO;

namespace DEiXTo.Services
{
    public class ExtractionPatternFileRepository : IExtractionPatternRepository
    {
        private readonly string _filename;

        public ExtractionPatternFileRepository(string filename)
        {
            _filename = filename;
        }

        public ExtractionPattern Load()
        {
            return null;
        }

        public void Save(ExtractionPattern pattern)
        {
            using (var stream = new FileStream(_filename, FileMode.Open))
            {
                var writer = new ExtractionPatternWriter();
                writer.Write(stream, pattern);
            }
        }
    }
}
