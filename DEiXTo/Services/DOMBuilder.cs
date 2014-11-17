using System.Linq;
using DEiXTo.Models;
using mshtml;
using System;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public class DOMBuilder
    {
        public DOMTree BuildDOMTree(HtmlElement element)
        {
            var domNode = element.DomElement as IHTMLDOMNode;
            var rootNode = new TreeNode();
            DOMTree domTree = new DOMTree();
            BuildDOMTreeRec(domNode, rootNode, domTree);
            domTree.RootNode = rootNode.FirstNode;
            return domTree;
        }

        public DOMTree BuildSimplifiedDOMTree(HtmlElement element, string[] ignoredTags)
        {
            var domNode = element.DomElement as IHTMLDOMNode;
            var rootNode = new TreeNode();
            DOMTree domTree = new DOMTree();
            BuildSimpliefiedDOMTreeRec(domNode, rootNode, domTree, ignoredTags);
            domTree.RootNode = rootNode.FirstNode;
            return domTree;
        }

        private bool IsIgnoredTag(IHTMLDOMNode element, string[] ignoredTags)
        {
            string tag = "<" + element.nodeName.ToUpper() + ">";

            return ignoredTags.Contains(tag);
        }

        private bool IsTextNode(TreeNode node)
        {
            return node != null && node.Text == "TEXT";
        }

        private void AddTextNode(IHTMLDOMNode element, TreeNode node, DOMTree domTree, string[] ignoredTags, NodeInfo pInfo)
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
                first.ToolTipText += value.Trim();

                return;
            }

            for (int i = 0; i < len; i++)
            {
                curElement = childrenElements.item(i);
                value = curElement.nodeValue as string;

                if (curElement.nodeName == "#text" && !String.IsNullOrWhiteSpace(value))
                {
                    var txtNode = new TreeNode("TEXT");
                    txtNode.ToolTipText = value.Trim();
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

        private void BuildSimpliefiedDOMTreeRec(IHTMLDOMNode element, TreeNode node, DOMTree domTree, string[] ignoredTags)
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

        private void InsertChildNodesIgnored(IHTMLDOMNode element, NodeInfo pInfo, TreeNode tmpNode, DOMTree domTree, string[] ignoredTags)
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
                    txtNode.ToolTipText = value.Trim();
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

        private bool IgnoredElement(IHTMLDOMNode element)
        {
            return element.nodeName == "#text" || element.nodeName == "#comment";
        }

        private TreeNode InsertNode(IHTMLDOMNode element, TreeNode node, DOMTree domTree)
        {
            var newNode = node.Nodes.Add(element.nodeName);
            domTree.Add(element, newNode);

            return newNode;
        }

        private void ApplyGrayedState(TreeNode node)
        {
            node.ImageIndex = 3;
            node.SelectedImageIndex = 3;
        }

        private void SetNodeInfo(TreeNode newNode, IHTMLDOMNode element, NodeInfo pInfo, TreeNode node)
        {
            var tmpElem = (IHTMLElement)element;

            pInfo.Attributes = AttributeExtractionFactory.GetExtractorFor(tmpElem).Attributes();
            pInfo.SourceIndex = tmpElem.sourceIndex;
            pInfo.Path = ComputePath(node, tmpElem);
            pInfo.Content = ContentExtractionFactory.GetExtractorFor(tmpElem).ExtractContent();
            pInfo.State = NodeState.Grayed;
            pInfo.Source = tmpElem.outerHTML;
            
            newNode.Tag = pInfo;
            newNode.ToolTipText = GetTooltipFor(tmpElem);
        }

        private void InsertChildNodes(IHTMLDOMNode element, NodeInfo pInfo, TreeNode newNode, DOMTree domTree)
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

        private void SetChildNodeInfo(IHTMLDOMNode curElement, NodeInfo pInfo, TreeNode newNode)
        {
            var txtNode = new TreeNode("TEXT");
            string value = curElement.nodeValue as string;
            txtNode.ToolTipText = value.Trim();
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

        private bool IsTextNode(IHTMLDOMNode element, string value)
        {
            return element.nodeName == "#text" && !String.IsNullOrWhiteSpace(value);
        }

        private void BuildDOMTreeRec(IHTMLDOMNode element, TreeNode node, DOMTree domTree)
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
