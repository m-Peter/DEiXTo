using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public class SiblingOrderAdded
    {
        public int StartIndex { get; set; }
        public int StepValue { get; set; }
        public TreeNode Node { get; set; }

        public SiblingOrderAdded(int startIndex, int stepValue, TreeNode node)
        {
            StartIndex = startIndex;
            StepValue = stepValue;
            Node = node;
        }
    }
}
