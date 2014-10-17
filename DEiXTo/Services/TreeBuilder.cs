using System.Linq;
using DEiXTo.Models;
using mshtml;
using System;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    /// <summary>
    /// This class builds the DOM tree structure from a page and maps HtmlElements
    /// to TreeNodes.
    /// </summary>
    public class TreeBuilder
    {
        /// <summary>
        /// Build the DOMTreeStructure for the given HtmlElement.
        /// </summary>
        /// <param name="element">The root HtmlElement</param>
        /// <returns>The DOMTreeStructure representation</returns>
        public DOMTreeStructure BuildDOMTree(HtmlElement element)
        {
            var domNode = element.DomElement as IHTMLDOMNode;
            var rootNode = new TreeNode();
            DOMTreeStructure domTree = new DOMTreeStructure();
            BuildDOMTreeRec(domNode, rootNode, domTree);
            domTree.RootNode = rootNode.FirstNode;
            return domTree;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="ignoredTags"></param>
        /// <returns></returns>
        public DOMTreeStructure BuildSimplifiedDOMTree(HtmlElement element, string[] ignoredTags)
        {
            var domNode = element.DomElement as IHTMLDOMNode;
            var rootNode = new TreeNode();
            DOMTreeStructure domTree = new DOMTreeStructure();
            BuildSimpliefiedDOMTreeRec(domNode, rootNode, domTree, ignoredTags);
            domTree.RootNode = rootNode.FirstNode;
            return domTree;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="node"></param>
        /// <param name="domTree"></param>
        /// <param name="ignoredTags"></param>
        private void BuildSimpliefiedDOMTreeRec(IHTMLDOMNode element, TreeNode node, DOMTreeStructure domTree, string[] ignoredTags)
        {
            if (element.nodeName == "#text" || element.nodeName == "#comment")
            {
                return;
            }

            IHTMLDOMChildrenCollection childrenElements = element.childNodes as IHTMLDOMChildrenCollection;
            int len = childrenElements.length;
            IHTMLDOMNode curElement;
            string value;

            string tag = "<" + element.nodeName.ToUpper() + ">";
            if (ignoredTags.Contains(tag))
            {
                for (int i = 0; i < len; i++)
                {
                    curElement = childrenElements.item(i);
                    value = curElement.nodeValue as string;
                    if (curElement.nodeName == "#text" && !String.IsNullOrWhiteSpace(value))
                    {
                        var txtNode = new TreeNode("TEXT");
                        txtNode.ToolTipText = curElement.nodeValue;
                        node.Nodes.Add(txtNode);
                    }
                    BuildSimpliefiedDOMTreeRec(curElement, node, domTree, ignoredTags);
                }
                return;
            }

            var tmpNode = node.Nodes.Add(element.nodeName);

            if (!domTree.ContainsKey(element))
            {
                domTree.Add(element, tmpNode);
            }

            NodeInfo pInfo = new NodeInfo();

            var tmpElem = (IHTMLElement)element;

            pInfo.ElementSourceIndex = tmpElem.sourceIndex;
            pInfo.Path = ComputePath(node, tmpElem);
            pInfo.Content = ContentExtractionFactory.GetExtractorFor(tmpElem).ExtractContent();
            tmpNode.Tag = pInfo;
            tmpNode.ToolTipText = GetTooltipFor(tmpElem);

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
                BuildSimpliefiedDOMTreeRec(curElement, tmpNode, domTree, ignoredTags);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="node"></param>
        /// <param name="domTree"></param>
        private void BuildDOMTreeRec(IHTMLDOMNode element, TreeNode node, DOMTreeStructure domTree)
        {
            if (element.nodeName == "#text" || element.nodeName == "#comment")
            {
                return;
            }

            var newNode = node.Nodes.Add(element.nodeName);

            if (!domTree.ContainsKey(element))
            {
                domTree.Add(element, newNode);
            }

            NodeInfo pInfo = new NodeInfo();

            // By default each node is required, so add the Match Node image
            newNode.ImageIndex = 3;
            newNode.SelectedImageIndex = 3;
            var tmpElem = (IHTMLElement)element;

            pInfo.ElementSourceIndex = tmpElem.sourceIndex;
            pInfo.Path = ComputePath(node, tmpElem);
            pInfo.Content = ContentExtractionFactory.GetExtractorFor(tmpElem).ExtractContent();
            pInfo.State = NodeState.Grayed;
            pInfo.Source = tmpElem.outerHTML;
            newNode.Tag = pInfo;
            newNode.ToolTipText = GetTooltipFor(tmpElem);

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
                    NodeInfo pointer = new NodeInfo();
                    pointer.Path = pInfo.Path + ".TEXT";
                    pointer.Content = curElement.nodeValue;
                    pointer.State = NodeState.Checked;
                    // By default, each TEXT node is in match and extract content state.
                    txtNode.ImageIndex = 0;
                    txtNode.SelectedImageIndex = 0;
                    txtNode.Tag = pointer;
                    newNode.Nodes.Add(txtNode);
                }
                BuildDOMTreeRec(curElement, newNode, domTree);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private string GetTooltipFor(IHTMLElement element)
        {
            var tagName = element.tagName;
            var tooltip = "";

            switch (tagName)
            {
                case "A":
                    tooltip = element.getAttribute("href");
                    break;
                case "IMG":
                    tooltip = element.getAttribute("src");
                    break;
                case "FORM":
                case "INPUT":
                    tooltip = element.getAttribute("name");
                    break;
            }

            return tooltip;
        }
    }
}
