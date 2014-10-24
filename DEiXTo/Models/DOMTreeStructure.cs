using mshtml;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DEiXTo.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class DOMTreeStructure
    {
        #region Instance Variables
        private IDictionary<IHTMLDOMNode, TreeNode> _DOMTree = new Dictionary<IHTMLDOMNode, TreeNode>();
        private TreeNode _rootNode;
        #endregion

        #region Properties
        /// <summary>
        /// The root TreeNode of the DOM tree representation.
        /// </summary>
        public TreeNode RootNode
        {
            get { return _rootNode; }
            set { _rootNode = value; }
        }
        #endregion

        #region Public Methods
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public TreeNode GetNodeFor(HtmlElement element)
        {
            var curElem = element.DomElement as IHTMLDOMNode;

            if (ContainsKey(curElem))
            {
                return _DOMTree[curElem];
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(IHTMLDOMNode key, TreeNode value)
        {
            if (ContainsKey(key))
            {
                return;
            }

            _DOMTree.Add(key, value);
        }

        /// <summary>
        /// Clear all the nodes.
        /// </summary>
        public void ClearDOM()
        {
            _DOMTree.Clear();
        }
        #endregion
    }
}
