using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class DOMTreeStructure
    {
        private IDictionary<IHTMLDOMNode, TreeNode> _DOMTree = new Dictionary<IHTMLDOMNode, TreeNode>();
        private TreeNode _rootNode;

        /// <summary>
        /// The root TreeNode of the DOM tree representation.
        /// </summary>
        public TreeNode RootNode
        {
            get { return _rootNode; }
            set { _rootNode = value; }
        }

        /// <summary>
        /// Count the number of elements.
        /// </summary>
        /// <returns></returns>
        public int CountElements()
        {
            return _DOMTree.Count;
        }

        /// <summary>
        /// Returns whether the given key is contained.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(IHTMLDOMNode key)
        {
            return _DOMTree.ContainsKey(key);
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
            _DOMTree.Add(key, value);
        }

        /// <summary>
        /// Clear all the nodes.
        /// </summary>
        public void ClearDOM()
        {
            _DOMTree.Clear();
        }
    }
}
