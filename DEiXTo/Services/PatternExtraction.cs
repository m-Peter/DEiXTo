using DEiXTo.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public class PatternExtraction
    {
        private TreeNode _pattern;
        private TreeNodeCollection _domNodes;
        private int _counter;
        private List<Result> _results;

        public PatternExtraction(TreeNode pattern, TreeNodeCollection domNodes)
        {
            _pattern = pattern;
            _domNodes = domNodes;
            _counter = 0;
            _results = new List<Result>();
        }

        public void FindMatches()
        {
            _counter = 0;
            _results = new List<Result>();
            FindDOMMatches(_pattern, _domNodes);
        }

        public int Results
        {
            get { return _counter; }
        }

        public List<Result> ExtractedResults()
        {
            return _results;
        }

        private void FindDOMMatches(TreeNode pattern, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (CompareRecursiveTree(pattern, node))
                {
                    _counter += 1;
                    // this is where the exact matching has succeeded and node
                    // is a tree that matched.
                    var result = new Result();
                    GetResultFromInstance(node, result);
                    _results.Add(result);
                }
                FindDOMMatches(pattern, node.Nodes);
            }
        }

        private void GetResultFromInstance(TreeNode node, Result result)
        {
            if (node.Text == "TEXT")
            {
                result.AddContent(node.GetContent());
            }
            foreach (TreeNode n in node.Nodes)
            {
                GetResultFromInstance(n, result);
            }
        }

        private bool CompareRecursiveTree(TreeNode left, TreeNode right)
        {
            if (left == null || right == null)
            {
                return false;
            }

            if ((left.Text != right.Text) || (left.Nodes.Count != right.Nodes.Count))
            {
                return false;
            }

            for (int i = 0; i < left.Nodes.Count; i++)
            {
                if (!CompareRecursiveTree(left.Nodes[i], right.Nodes[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
