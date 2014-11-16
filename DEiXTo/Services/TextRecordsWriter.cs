using System.Collections.Generic;
using System.IO;
using DEiXTo.Models;

namespace DEiXTo.Services
{
    public class TextRecordsWriter : ExtractedRecordsWriter
    {
        private StreamWriter _file;

        public TextRecordsWriter(string filename)
        {
            _filename = filename;
        }

        public override void Write(IEnumerable<Result> results)
        {
            using (_file = new StreamWriter(@_filename))
            {
                WriteResults(results);
            }
        }

        private void WriteResults(IEnumerable<Result> results)
        {
            foreach (Result record in results)
            {
                WriteContent(record);

                _file.WriteLine();
            }
        }

        private void WriteContent(Result record)
        {
            foreach (string content in record.Contents())
            {
                _file.Write(content);
                _file.Write("\t");
            }
        }
    }
}
