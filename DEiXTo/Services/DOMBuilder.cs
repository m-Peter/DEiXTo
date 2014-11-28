using System.Linq;
using DEiXTo.Models;
using mshtml;
using System;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public class DOMBuilder : IDOMBuilder
    {
        private HtmlElement _element;

        public DOMBuilder(HtmlElement element)
        {
            _element = element;
        }

        public DOMTree Build()
        {
            var domNode = _element.DomElement as IHTMLDOMNode;
            var rootNode = new TreeNode();
            DOMTree domTree = new DOMTree();
            BuildDOMTreeRec(domNode, rootNode, domTree);
            domTree.RootNode = rootNode.FirstNode;
            return domTree;
        }

        private bool IsTextNode(TreeNode node)
        {
            return node != null && node.Text == "TEXT";
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
            newNode.ToolTipText = TooltipExtractionFactory.GetTooltipFor(element).ExtractTooltip();
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
            txtNode.ToolTipText = TooltipExtractionFactory.GetTooltipFor(curElement).ExtractTooltip();
            NodeInfo pointer = new NodeInfo();
            pointer.IsTextNode = true;
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
            return path.Substring(1);
        }
    }
}
