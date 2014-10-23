using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DEiXTo.Models;

namespace DEiXTo.Services
{
    public class WriteExtractedRecords
    {
        private string _filename;
        private StreamWriter _file;

        public WriteExtractedRecords(string filename)
        {
            _filename = filename;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
        public void Write(IEnumerable<Result> results)
        {
            using (_file = new StreamWriter(@_filename))
            {
                WriteResults(results);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
        private void WriteResults(IEnumerable<Result> results)
        {
            foreach (Result record in results)
            {
                WriteContent(record);

                _file.WriteLine();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
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
