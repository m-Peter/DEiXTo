using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public class LabelAdded
    {
        public string Label { get; private set; }
        public TreeNode Node { get; private set; }

        public LabelAdded(string label, TreeNode node)
        {
            Label = label;
            Node = node;
        }
    }
}
