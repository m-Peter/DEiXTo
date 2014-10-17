using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Services
{
    public class LabelAdded
    {
        public string Label { get; private set; }

        public LabelAdded(string label)
        {
            Label = label;
        }
    }
}
