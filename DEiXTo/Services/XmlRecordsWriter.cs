using DEiXTo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Services
{
    public class XmlRecordsWriter : ExtractedRecordsWriter
    {
        public XmlRecordsWriter(string filename)
        {
            _filename = filename;
        }

        public override void Write(IEnumerable<Result> results)
        {
            
        }
    }
}
