using System;
using System.Collections.Generic;

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
