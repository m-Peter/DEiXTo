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
        /// <param name="ignoredTags"></param>
        /// <returns></returns>
        private bool IsIgnoredTag(IHTMLDOMNode element, string[] ignoredTags)
        {
            string tag = "<" + element.nodeName.ToUpper() + ">";

            return ignoredTags.Contains(tag);
        }

        private bool IsTextNode(TreeNode node)
        {
            return node != null && node.Text == "TEXT";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="node"></param>
        /// <param name="domTree"></param>
        /// <param name="ignoredTags"></param>
        /// <param name="pInfo"></param>
        private void AddTextNode(IHTMLDOMNode element, TreeNode node, DOMTreeStructure domTree, string[] ignoredTags, NodeInfo pInfo)
        {
            IHTMLDOMChildrenCollection childrenElements = element.childNodes as IHTMLDOMChildrenCollection;
            int len = childrenElements.length;
            IHTMLDOMNode curElement;
            string value;

            var first = node.FirstNode;

            if (IsTextNode(first))
            {
                curElement = childrenElements.item(0);
                value = curElement.nodeValue as string;
                var content = first.GetContent();
                first.SetContent(content + value);
                first.ToolTipText += value;

                return;
            }

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
                    txtNode.ImageIndex = 0;
                    txtNode.SelectedImageIndex = 0;
                    txtNode.Tag = pointer;
                    node.Nodes.Add(txtNode);
                }

                BuildSimpliefiedDOMTreeRec(curElement, node, domTree, ignoredTags);
            }
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
            // P
            //    TEXT
            //    EM (ignored)
            //        TEXT
            // 
            // (1) First we encounter the P tag, it's not an IgnoredElement or IsIgnoredTag, so we continue and
            // add its child nodes.
            // (2) Next we encounter the TEXT child node of P. It is an IgnoredElement, so we return. TEXT nodes
            // do not have any childrens, that's the reason we return.
            // (3) Next we encounter the EM child node of P. It is an IgnoredTag, so we only add his TEXT child,
            // if it has one. In this case there is a TEXT child node on EM. But, we're adding the TEXT node in
            // the parent node of EM. This way, the P tag (parent of EM) will end up with two TEXT nodes. In any
            // case we, a node should have only one TEXT child node. In order to achieve this invariant, we have
            // to check whether the parent already has a TEXT child node. If yes, we should merge the TEXT child
            // node of EM.

            if (IgnoredElement(element))
            {
                return;
            }

            NodeInfo pInfo = new NodeInfo();

            if (IsIgnoredTag(element, ignoredTags))
            {
                // element is the EM tag, node is the P tag
                AddTextNode(element, node, domTree, ignoredTags, pInfo);

                return;
            }

            var tmpNode = InsertNode(element, node, domTree);

            ApplyGrayedState(tmpNode);
            
            SetNodeInfo(tmpNode, element, pInfo, node);

            InsertChildNodesIgnored(element, pInfo, tmpNode, domTree, ignoredTags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="pInfo"></param>
        /// <param name="tmpNode"></param>
        /// <param name="domTree"></param>
        /// <param name="ignoredTags"></param>
        private void InsertChildNodesIgnored(IHTMLDOMNode element, NodeInfo pInfo, TreeNode tmpNode, DOMTreeStructure domTree, string[] ignoredTags)
        {
            IHTMLDOMChildrenCollection childrenElements = element.childNodes as IHTMLDOMChildrenCollection;
            int len = childrenElements.length;
            IHTMLDOMNode curElement;
            string value;

            for (int i = 0; i < len; i++)
            {
                curElement = childrenElements.item(i);
                value = curElement.nodeValue as string;

                if (IsTextNode(curElement, value))
                {
                    var txtNode = new TreeNode("TEXT");
                    txtNode.ToolTipText = curElement.nodeValue;
                    NodeInfo pointer = new NodeInfo();
                    pointer.Path = pInfo.Path + ".TEXT";
                    pointer.Content = curElement.nodeValue;
                    pointer.State = NodeState.Checked;
                    txtNode.ImageIndex = 0;
                    txtNode.SelectedImageIndex = 0;
                    txtNode.Tag = pointer;
                    tmpNode.Nodes.Add(txtNode);
                }

                BuildSimpliefiedDOMTreeRec(curElement, tmpNode, domTree, ignoredTags);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private bool IgnoredElement(IHTMLDOMNode element)
        {
            return element.nodeName == "#text" || element.nodeName == "#comment";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="node"></param>
        /// <param name="domTree"></param>
        private TreeNode InsertNode(IHTMLDOMNode element, TreeNode node, DOMTreeStructure domTree)
        {
            var newNode = node.Nodes.Add(element.nodeName);
            domTree.Add(element, newNode);

            return newNode;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ApplyGrayedState(TreeNode node)
        {
            node.ImageIndex = 3;
            node.SelectedImageIndex = 3;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetNodeInfo(TreeNode newNode, IHTMLDOMNode element, NodeInfo pInfo, TreeNode node)
        {
            var tmpElem = (IHTMLElement)element;

            pInfo.ElementSourceIndex = tmpElem.sourceIndex;
            pInfo.Path = ComputePath(node, tmpElem);
            pInfo.Content = ContentExtractionFactory.GetExtractorFor(tmpElem).ExtractContent();
            pInfo.State = NodeState.Grayed;
            pInfo.Source = tmpElem.outerHTML;
            newNode.Tag = pInfo;
            newNode.ToolTipText = GetTooltipFor(tmpElem);
        }

        /// <summary>
        /// 
        /// </summary>
        private void InsertChildNodes(IHTMLDOMNode element, NodeInfo pInfo, TreeNode newNode, DOMTreeStructure domTree)
        {
            IHTMLDOMChildrenCollection childrenElements = element.childNodes as IHTMLDOMChildrenCollection;
            int len = childrenElements.length;
            IHTMLDOMNode curElement;
            string value;

            for (int i = 0; i < len; i++)
            {
                curElement = childrenElements.item(i);
                value = curElement.nodeValue as string;

                if (IsTextNode(curElement, value))
                {
                    SetChildNodeInfo(curElement, pInfo, newNode);
                }

                BuildDOMTreeRec(curElement, newNode, domTree);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="curElement"></param>
        /// <param name="pInfo"></param>
        /// <param name="newNode"></param>
        private void SetChildNodeInfo(IHTMLDOMNode curElement, NodeInfo pInfo, TreeNode newNode)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsTextNode(IHTMLDOMNode element, string value)
        {
            return element.nodeName == "#text" && !String.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="node"></param>
        /// <param name="domTree"></param>
        private void BuildDOMTreeRec(IHTMLDOMNode element, TreeNode node, DOMTreeStructure domTree)
        {
            if (IgnoredElement(element))
            {
                return;
            }

            var newNode = InsertNode(element, node, domTree);
            NodeInfo pInfo = new NodeInfo();
            // By default each node is required, so add the Match Node image
            ApplyGrayedState(newNode);

            SetNodeInfo(newNode, element, pInfo, node);
            
            InsertChildNodes(element, pInfo, newNode, domTree);
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

            if (node == null)
            {
                path = "HTML[1]";
            }

            bro = node.FirstNode;

            if (bro == null)
            {
                path = node.GetPath() + String.Format(".{0}[1]", element.tagName);
            }

            while (bro != null)
            {
                if (bro.Text == element.tagName)
                {
                    cnt += 1;
                }
                bro = bro.NextNode;
            }

            path = node.GetPath() + String.Format(".{0}[{1}]", element.tagName, cnt);

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
