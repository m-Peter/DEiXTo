using DEiXTo.Models;
using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    /// <summary>
    /// This class builds the DOM tree structure from a page and maps HtmlElements
    /// to TreeNodes.
    /// </summary>
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

        /*public TreeNode TreeNodeFromElement(HtmlElement element)
        {
            var tmpElem = (IHTMLDOMNode)element.DomElement;
            var node = _domTree[tmpElem];

            return node;
        }*/

        public TreeNode GetNodeFor(HtmlElement element)
        {
            var curElem = element.DomElement as IHTMLDOMNode;

            return _domTree[curElem];
        }

        public string ComputePath(TreeNode node, IHTMLElement element)
        {
            TreeNode bro;
            int cnt = 0;
            string path = "";

            if (node != null)
            {
                bro = node.FirstNode;
                if (bro != null)
                {
                    while (bro != null)
                    {
                        if (bro.Text == element.tagName)
                        {
                            cnt += 1;
                        }
                        bro = bro.NextNode;
                    }

                    path = node.GetPath() + String.Format(".{0}[{1}]", element.tagName, cnt);
                }
                else
                {
                    path = node.GetPath() + String.Format(".{0}[1]", element.tagName);
                }
            }
            else
            {
                path = "HTML[1]";
            }

            return path;
        }

        private string GetContentFor(IHTMLElement element)
        {
            var tagName = element.tagName;
            var content = "";

            switch (tagName)
            {
                case "A":
                    content = element.getAttribute("href");
                    break;
                case "IMG":
                    content = element.getAttribute("src");
                    break;
                case "FORM":
                case "INPUT":
                    content = element.getAttribute("name");
                    break;
                default:
                    content = element.innerText;
                    break;
            }

            return content;
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

            PointerInfo pInfo = new PointerInfo();

            var tmpElem = (IHTMLElement)element;

            pInfo.ElementSourceIndex = tmpElem.sourceIndex;
            pInfo.Path = ComputePath(treeNode, tmpElem);
            pInfo.Content = GetContentFor(tmpElem);
            tmpNode.Tag = pInfo;

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
                    tmpNode.Nodes.Add(txtNode);
                }
                BuildDomTree(curElement, tmpNode, false);
            }
        }
    }
}
