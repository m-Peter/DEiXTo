using DEiXTo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using mshtml;
using DEiXTo.Services;

namespace DEiXTo.Presenters
{
    public class DeixtoAgentPresenter
    {
        private readonly IDeixtoAgentView _view;
        private ElementStyling _styling;

        public DeixtoAgentPresenter(IDeixtoAgentView view)
        {
            _view = view;
            _styling = new ElementStyling();

            // ATTACH THE EVENTS OF THE VIEW TO LOCAL METHODS
            _view.BrowseToUrl += _view_BrowseToUrl;
            _view.KeyDownPress += _view_KeyDownPress;
            _view.AutoFillChanged += _view_AutoFillChanged;
            _view.CrawlingChanged += _view_CrawlingChanged;
            _view.BrowserCompleted += _view_BrowserCompleted;
            _view.DocumentMouseOver += _view_DocumentMouseOver;
            _view.DocumentMouseLeave += _view_DocumentMouseLeave;
        }

        

        private TreeNode BuildDom(HtmlElement element)
        {
            var curElem = element.DomElement as IHTMLDOMNode;
            var rootNode = new TreeNode();
            BuildDomTree(curElem, rootNode, true);

            return rootNode.FirstNode;
        }

        private void BuildDomTree(IHTMLDOMNode element, TreeNode treeNode, bool IsRoot = false)
        {
            if (element.nodeName == "#text" || element.nodeName == "#comment")
            {
                return;
            }

            var tmpNode = treeNode.Nodes.Add(element.nodeName);
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

        void _view_DocumentMouseLeave(HtmlElement element)
        {
            _styling.Unstyle(element);
        }

        void _view_DocumentMouseOver(HtmlElement element)
        {
            _styling.UnstyleElements();
            _styling.Style(element);
        }

        void _view_CrawlingChanged(bool state)
        {
            _view.ApplyVisibilityStateInCrawling(state);
        }

        void _view_AutoFillChanged(bool state)
        {
            _view.ApplyVisibilityStateInAutoFill(state);
        }

        void _view_KeyDownPress(KeyEventArgs e)
        {
            if (e.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Right:
                        _view.NavigateForward();
                        break;
                    case Keys.Left:
                        _view.NavigateBack();
                        break;
                }
                e.Handled = true;
            }
        }

        void _view_BrowseToUrl()
        {
            var url = _view.Url;

            if (String.IsNullOrWhiteSpace(url))
            {
                _view.ShowWarningMessage();
            }

            _view.NavigateTo(url);
        }

        void _view_BrowserCompleted()
        {
            var elem = _view.GetHTMLElement();
            var rootNode = BuildDom(elem);
            _view.FillDomTree(rootNode);
        }
    }
}
