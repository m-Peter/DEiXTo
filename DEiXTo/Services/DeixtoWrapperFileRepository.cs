using DEiXTo.Models;
using System.IO;
using System.Xml;

namespace DEiXTo.Services
{
    public class DeixtoWrapperFileRepository : IDeixtoWrapperRepository
    {
        private readonly string _filename;
        private readonly IDeixtoWrapperMapper _mapper;

        public DeixtoWrapperFileRepository(string filename)
        {
            _filename = filename;
            _mapper = new DeixtoWrapperMapper();
        }

        public DeixtoWrapper Load(Stream stream)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            var reader = XmlReader.Create(stream, settings);

            return _mapper.Map(reader);
        }

        public void Save(DeixtoWrapper wrapper, Stream stream)
        {
            var writer = new DeixtoWrapperWriter(wrapper);

            writer.Write(stream);
        }
    }
}
