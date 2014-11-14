using DEiXTo.Models;
using System.Collections.Generic;
using System.Xml;

namespace DEiXTo.Services
{
    public class XmlRecordsWriter : ExtractedRecordsWriter
    {
        private XmlWriter _writer;

        public XmlRecordsWriter(string filename)
        {
            _filename = filename;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
        public override void Write(IEnumerable<Result> results)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (_writer = XmlWriter.Create(_filename, settings))
            {
                _writer.WriteStartDocument(); // Write the first line

                _writer.WriteStartElement("DEiXToExtractedContent"); // Write DEiXToExtractedContent element

                WriteItems(results);

                _writer.WriteEndElement(); // Close DEiXToExtractedContent element
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
        private void WriteItems(IEnumerable<Result> results)
        {
            _writer.WriteStartElement("items"); // Write the items element

            foreach (Result result in results)
            {
                WriteItem(result);
            }

            _writer.WriteEndElement(); // Close items element
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="result"></param>
        private void WriteItem(Result result)
        {
            _writer.WriteStartElement("item"); // Write the item element

            WriteVariables(result);

            _writer.WriteEndElement(); // Close the item element
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="result"></param>
        private void WriteVariables(Result result)
        {
            int i = 0;
            string format = "VAR";

            foreach (string content in result.Contents())
            {
                i++;

                _writer.WriteElementString(format + i, content);
            }
        }
    }
}
