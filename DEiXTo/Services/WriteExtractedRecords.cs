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
            using (StreamWriter file = new StreamWriter(@_filename))
            {
                foreach (Result record in results)
                {
                    foreach (string content in record.Contents())
                    {
                        file.Write(content);
                        file.Write("\t");
                    }

                    file.WriteLine();
                }
            }
        }
    }
}
