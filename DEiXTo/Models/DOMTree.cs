using mshtml;
using System.Collections.Generic;
using System.Windows.Forms;
using DEiXTo.Services;

namespace DEiXTo.Models
{
    public class DOMTree
    {
        private IDictionary<IHTMLDOMNode, TreeNode> _DOMTree = new Dictionary<IHTMLDOMNode, TreeNode>();
        private TreeNode _rootNode;

        public TreeNode RootNode
        {
            get { return _rootNode; }
            set { _rootNode = value; }
        }

        public int CountElements()
        {
            return _DOMTree.Count;
        }

        public bool ContainsKey(IHTMLDOMNode key)
        {
            return _DOMTree.ContainsKey(key);
        }

        public TreeNode ScanTree(TreeNode pattern)
        {
            var nodes = _DOMTree.Values;
            
            foreach (var node in nodes)
            {
                if (CompareTrees(node, pattern))
                {
                    return node.GetClone();
                }
            }

            return null;
        }

        public TreeNode GetNodeFor(HtmlElement element)
        {
            var curElem = element.DomElement as IHTMLDOMNode;

            if (ContainsKey(curElem))
            {
                return _DOMTree[curElem];
            }

            return null;
        }

        public void Add(IHTMLDOMNode key, TreeNode value)
        {
            if (ContainsKey(key))
            {
                return;
            }

            _DOMTree.Add(key, value);
        }

        public void ClearDOM()
        {
            _DOMTree.Clear();
        }

        private bool CompareTrees(TreeNode left, TreeNode right)
        {
            if (left.Text != right.Text)
            {
                return false;
            }

            for (int i = 0; i < left.Nodes.Count; i++)
            {
                var nextLeft = left.Nodes[i];
                bool hasNode = HasNextNode(right, i);

                if (hasNode)
                {
                    return false;
                }

                var nextRight = right.Nodes[i];

                if (!CompareTrees(nextLeft, nextRight))
                {
                    return false;
                }
            }

            return true;
        }

        private bool HasNextNode(TreeNode node, int index)
        {
            return node.Nodes.Count <= index;
        }
    }
}
