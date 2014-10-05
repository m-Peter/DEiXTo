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
        private DOMBuilder _builder;
        private DocumentQuery _document;

        public DeixtoAgentPresenter(IDeixtoAgentView view)
        {
            _view = view;
            _styling = new ElementStyling();
            _builder = new DOMBuilder();

            // ATTACH THE EVENTS OF THE VIEW TO LOCAL METHODS
            _view.BrowseToUrl += _view_BrowseToUrl;
            _view.KeyDownPress += _view_KeyDownPress;
            _view.AutoFillChanged += _view_AutoFillChanged;
            _view.CrawlingChanged += _view_CrawlingChanged;
            _view.BrowserCompleted += _view_BrowserCompleted;
            _view.DocumentMouseOver += _view_DocumentMouseOver;
            _view.DocumentMouseLeave += _view_DocumentMouseLeave;
            _view.DOMNodeClick += _view_DOMNodeClick;
            _view.CreateWorkingPattern += _view_CreateWorkingPattern;
            _view.CreateAuxiliaryPattern += _view_CreateAuxiliaryPattern;
            _view.WorkingPatternNodeClick += _view_WorkingPatternNodeClick;
            _view.CreateWorkingPatternFromDocument += _view_CreateWorkingPatternFromDocument;
            _view.CreateAuxiliaryPatternFromDocument += _view_CreateAuxiliaryPatternFromDocument;
            _view.ShowBrowserContextMenu += _view_ShowBrowserContextMenu;
            _view.AuxiliaryPatternNodeClick += _view_AuxiliaryPatternNodeClick;
            _view.SimplifyDOMTree += _view_SimplifyDOMTree;
        }

        void _view_SimplifyDOMTree()
        {
            _builder.ClearDOM();
            _view.ClearDOMTree();
            _document = new DocumentQuery(_view.GetHTMLDocument());
            var elem = _document.GetHTMLElement();
            // Build the DOM tree structure from the HTML element of the page
            var ignoredTags = _view.IgnoredTags();
            var rootNode = _builder.BuildSimplifiedDom(elem, ignoredTags);
            // Assign the DOM tree structure to the DOM TreeView
            _view.FillDomTree(rootNode);
            // Append the URL of the current page to the TargetURLs container
            _view.AppendTargetUrl(_view.Url);
        }

        void _view_AuxiliaryPatternNodeClick(TreeNode node)
        {
            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);

            _styling.UnstyleElements();
            _styling.Style(element);

            var path = node.GetPath();

            _view.FillElementInfo(node, element.OuterHtml);
            _view.SelectDOMNode(_builder.GetNodeFor(element));

            if (_view.CanAutoScroll())
            {
                element.ScrollIntoView(false);
            }
        }

        void _view_CreateAuxiliaryPatternFromDocument(HtmlElement element)
        {
            _view.ClearAuxiliaryTree();

            var node = _builder.GetNodeFor(element);

            _view.FillAuxiliaryTree((TreeNode)node.Clone());

            _view.ExpandAuxiliaryTree();
        }

        void _view_ShowBrowserContextMenu(object sender, HtmlElementEventArgs e)
        {
            e.ReturnValue = e.CtrlKeyPressed;

            if (e.CtrlKeyPressed)
            {
                return;
            }

            if (!e.ReturnValue)
            {
                if (_view.BrowserContextMenuEnabled())
                {
                    _view.CurrentElement = _document.GetElementFromPoint(e.ClientMousePosition);
                    _view.ShowBrowserMenu();
                }
            }
        }

        void _view_CreateWorkingPatternFromDocument(HtmlElement element)
        {
            _view.ClearPatternTree();

            var node = _builder.GetNodeFor(element);

            _view.FillPatternTree((TreeNode)node.Clone());

            _view.ExpandPatternTree();
        }

        void _view_WorkingPatternNodeClick(TreeNode node, MouseButtons button)
        {
            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);

            if (_view.HighlightModeEnabled())
            {
                _styling.UnstyleElements();
                _styling.Style(element);
            }

            var path = node.GetPath();

            _view.FillElementInfo(node, element.OuterHtml);
            _view.SelectDOMNode(_builder.GetNodeFor(element));

            if (_view.CanAutoScroll())
            {
                element.ScrollIntoView(false);
            }
        }

        void _view_CreateAuxiliaryPattern(TreeNode node)
        {
            _view.ClearAuxiliaryTree();

            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);

            _view.FillAuxiliaryTree((TreeNode)node.Clone());

            _view.ExpandAuxiliaryTree();
        }

        void _view_CreateWorkingPattern(TreeNode node)
        {
            _view.ClearPatternTree();

            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);

            _view.FillPatternTree((TreeNode)node.Clone());

            _view.ExpandPatternTree();
        }

        void _view_DOMNodeClick(TreeNode node, MouseButtons button)
        {
            if (button == MouseButtons.Right)
            {
                _view.SetContextMenuFor(node);
            }

            _styling.UnstyleElements();
            // Retrieve the HTML element that corresponds to the DOM TreeNode
            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);
            // Style the HTML element in the WebBrowser
            _styling.Style(element);
            
            var path = node.GetPath();

            _view.FillElementInfo(node, element.OuterHtml);

            if (_view.CanAutoScroll())
            {
                element.ScrollIntoView(false);
            }
        }

        void _view_DocumentMouseLeave(HtmlElement element)
        {
            if (_view.HighlightModeEnabled())
            {
                _styling.Unstyle(element);
            }

            _view.ClearElementInfo();
        }

        void _view_DocumentMouseOver(HtmlElement element)
        {
            if (_view.HighlightModeEnabled())
            {
                _styling.UnstyleElements();
                _styling.Style(element);

                // Retrieve the TreeNode that corresponds to the given HTML element
                var node = _builder.GetNodeFor(element);

                // Scroll the TreeView to the specified TreeNode
                _view.SelectDOMNode(node);

                var path = node.GetPath();
                _view.FillElementInfo(node, element.OuterHtml);
            }
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
            else if (e.KeyCode == Keys.Enter)
            {
                _view_BrowseToUrl();
            }
        }

        void _view_BrowseToUrl()
        {
            var url = _view.Url;

            if (String.IsNullOrWhiteSpace(url))
            {
                _view.ShowWarningMessage();
                return;
            }

            _view.NavigateTo(url);
        }

        void _view_BrowserCompleted()
        {
            _view.ClearDOMTree();
            _document = new DocumentQuery(_view.GetHTMLDocument());
            var elem = _document.GetHTMLElement();
            // Build the DOM tree structure from the HTML element of the page
            var rootNode = _builder.BuildDom(elem);
            // Assign the DOM tree structure to the DOM TreeView
            _view.FillDomTree(rootNode);
            // Append the URL of the current page to the TargetURLs container
            _view.AppendTargetUrl(_view.Url);
        }
    }
}
