using DEiXTo.Models;
using System.Collections.Generic;

namespace DEiXTo.Services
{
    public abstract class ExtractedRecordsWriter
    {
        protected string _filename;

        public abstract void Write(IEnumerable<Result> results);
    }
}
