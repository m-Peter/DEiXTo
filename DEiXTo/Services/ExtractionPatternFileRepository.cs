using DEiXTo.Models;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace DEiXTo.Services
{
    public class ExtractionPatternFileRepository : IExtractionPatternRepository
    {
        private readonly string _filename;
        private readonly IExtractionPatternMapper _mapper;

        public ExtractionPatternFileRepository(string filename)
        {
            _filename = filename;
            _mapper = new ExtractionPatternMapper();
        }

        public ExtractionPattern Load(Stream stream)
        {
            var reader = XmlReader.Create(stream);

            return _mapper.Map(reader);
        }

        public void Save(ExtractionPattern pattern, Stream stream)
        {
            var writer = new ExtractionPatternWriter();

            writer.Write(stream, pattern);
        }
    }
}
