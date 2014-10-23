using DEiXTo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Services
{
    public abstract class ExtractedRecordsWriter
    {
        protected string _filename;

        public abstract void Write(IEnumerable<Result> results);
    }
}
