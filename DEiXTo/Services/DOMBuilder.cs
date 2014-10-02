using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public class DOMBuilder
    {
        private IDictionary<IHTMLDOMNode, TreeNode> _domTree = new Dictionary<IHTMLDOMNode, TreeNode>();

        public TreeNode BuildDom(HtmlElement element)
        {
            var curElem = element.DomElement as IHTMLDOMNode;
            var rootNode = new TreeNode();
            BuildDomTree(curElem, rootNode, true);

            return rootNode.FirstNode;
        }

        public TreeNode TreeNodeFromElement(HtmlElement element)
        {
            var tmpElem = (IHTMLDOMNode)element.DomElement;
            var node = _domTree[tmpElem];

            return node;
        }

        private void BuildDomTree(IHTMLDOMNode element, TreeNode treeNode, bool IsRoot = false)
        {
            if (element.nodeName == "#text" || element.nodeName == "#comment")
            {
                return;
            }

            var tmpNode = treeNode.Nodes.Add(element.nodeName);
            
            if (_domTree.ContainsKey(element) == false)
            {
                _domTree.Add(element, tmpNode);
            }

            var tmpElem = (IHTMLElement)element;

            IHTMLDOMChildrenCollection childrenElements = element.childNodes as IHTMLDOMChildrenCollection;
            int len = childrenElements.length;
            IHTMLDOMNode curElement;
            string value;

            for (int i = 0; i < len; i++)
            {
                curElement = childrenElements.item(i);
                value = curElement.nodeValue as string;
                if (curElement.nodeName == "#text" && !String.IsNullOrWhiteSpace(value))
                {
                    var txtNode = new TreeNode("TEXT");
                    txtNode.ToolTipText = curElement.nodeValue;
                    txtNode.ImageIndex = 0;
                    txtNode.SelectedImageIndex = 0;
                    tmpNode.Nodes.Add(txtNode);
                }
                BuildDomTree(curElement, tmpNode, false);
            }
        }
    }
}
