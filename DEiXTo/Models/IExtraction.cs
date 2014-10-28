using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Models
{
    public interface IExtraction
    {
        List<String> OutputVariableLabels { get; set; }
        IEnumerable<Result> ExtractedRecords { get; set; }
        int RecordsCount { get; }
        int VariablesCount { get; set; }
    }
}
