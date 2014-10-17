using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public class RegexAdded
    {
        public string Regex { get; private set; }
        public TreeNode Node { get; private set; }

        public RegexAdded(string regex, TreeNode node)
        {
            Regex = regex;
            Node = node;
        }
    }
}
