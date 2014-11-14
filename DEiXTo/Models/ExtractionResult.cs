using System;
using System.Collections.Generic;
using System.Linq;

namespace DEiXTo.Models
{
    public class ExtractionResult : IExtraction
    {
        private List<String> _outputVariableLabels;
        private IEnumerable<Result> _extractedRecords;
        private int _outputVariables;

        public List<String> OutputVariableLabels
        {
            get { return _outputVariableLabels; }
            set { _outputVariableLabels = value; }
        }

        public IEnumerable<Result> ExtractedRecords
        {
            get { return _extractedRecords; }
            set { _extractedRecords = value; }
        }

        public int RecordsCount
        {
            get { return _extractedRecords.Count(); }
        }

        public int VariablesCount
        {
            get { return _outputVariables; }
            set { _outputVariables = value; }
        }
    }
}
