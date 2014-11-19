using DEiXTo.Models;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace DEiXTo.Services
{
    public class ExtractionPatternFileRepository : IExtractionPatternRepository
    {
        private readonly string _filename;
        private readonly IFileLoader _loader;
        private readonly IExtractionPatternMapper _mapper;

        public ExtractionPatternFileRepository(string filename)
        {
            _filename = filename;
            //_loader = loader;
            _mapper = new ExtractionPatternMapper();
        }

        public ExtractionPattern Load(Stream stream)
        {
            /*using (var stream = _loader.Load(_filename, FileMode.Open))
            {
                var reader = XmlReader.Create(stream);

                return _mapper.Map(reader);
            }*/
            var reader = XmlReader.Create(stream);

            return _mapper.Map(reader);
        }

        public void Save(ExtractionPattern pattern, Stream stream)
        {
            /*using (var stream = _loader.Load(_filename, FileMode.CreateNew))
            {
                var writer = new ExtractionPatternWriter();

                writer.Write(stream, pattern);
            }*/

            var writer = new ExtractionPatternWriter();

            writer.Write(stream, pattern);
        }
    }
}
