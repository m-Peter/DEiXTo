using DEiXTo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using mshtml;
using DEiXTo.Services;
using System.Net;

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
            _view.BrowseToUrl += browseToUrl;
            _view.KeyDownPress += keyDownPress;
            _view.AutoFillChanged += autoFillChanged;
            _view.CrawlingChanged += crawlingChanged;
            _view.BrowserCompleted += browserCompleted;
            _view.DocumentMouseOver += documentMouseOver;
            _view.DocumentMouseLeave += documentMouseLeave;
            _view.DOMNodeClick += DOMNodeClick;
            _view.CreateWorkingPattern += createWorkingPattern;
            _view.CreateAuxiliaryPattern += createAuxiliaryPattern;
            _view.WorkingPatternNodeClick += workingPatternNodeClick;
            _view.CreateWorkingPatternFromDocument += createWorkingPatternFromDocument;
            _view.CreateAuxiliaryPatternFromDocument += createAuxiliaryPatternFromDocument;
            _view.ShowBrowserContextMenu += showBrowserContextMenu;
            _view.AuxiliaryPatternNodeClick += auxiliaryPatternNodeClick;
            _view.SimplifyDOMTree += simplifyDOMTree;
        }

        void simplifyDOMTree()
        {
            _builder.ClearDOM();
            _view.ClearDOMTree();
            _document = new DocumentQuery(_view.GetHtmlDocument());
            var elem = _document.GetHtmlElement();
            // Build the DOM tree structure from the HTML element of the page
            var ignoredTags = _view.IgnoredTags();
            var rootNode = _builder.BuildSimplifiedDom(elem, ignoredTags);
            // Assign the DOM tree structure to the DOM TreeView
            _view.FillDomTree(rootNode);
            // Append the URL of the current page to the TargetURLs container
            _view.AppendTargetUrl(_view.Url);
        }

        void auxiliaryPatternNodeClick(TreeNode node)
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

        void createAuxiliaryPatternFromDocument(HtmlElement element)
        {
            _view.ClearAuxiliaryTree();

            var node = _builder.GetNodeFor(element);

            _view.FillAuxiliaryTree((TreeNode)node.Clone());

            _view.ExpandAuxiliaryTree();
        }

        void showBrowserContextMenu(object sender, HtmlElementEventArgs e)
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

        void createWorkingPatternFromDocument(HtmlElement element)
        {
            _view.ClearPatternTree();

            var node = _builder.GetNodeFor(element);

            _view.FillPatternTree((TreeNode)node.Clone());

            _view.ExpandPatternTree();
        }

        void workingPatternNodeClick(TreeNode node, MouseButtons button)
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

        void createAuxiliaryPattern(TreeNode node)
        {
            _view.ClearAuxiliaryTree();

            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);

            _view.FillAuxiliaryTree((TreeNode)node.Clone());

            _view.ExpandAuxiliaryTree();
        }

        void createWorkingPattern(TreeNode node)
        {
            _view.ClearPatternTree();

            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);

            _view.FillPatternTree((TreeNode)node.Clone());

            _view.ExpandPatternTree();
        }

        void DOMNodeClick(TreeNode node, MouseButtons button)
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

        void documentMouseLeave(HtmlElement element)
        {
            if (_view.HighlightModeEnabled())
            {
                _styling.Unstyle(element);
            }

            _view.ClearElementInfo();
        }

        void documentMouseOver(HtmlElement element)
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

        void crawlingChanged(bool state)
        {
            _view.ApplyVisibilityStateInCrawling(state);
        }

        void autoFillChanged(bool state)
        {
            _view.ApplyVisibilityStateInAutoFill(state);
        }

        void keyDownPress(KeyEventArgs e)
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
                browseToUrl();
            }
        }

        /// <summary>
        /// Navigates the WebBrowser to the user specified URL. The URL can also
        /// refer to a local HTML document. If the download of a page fails for
        /// some reason, a message describing the error is shown. It clears all
        /// the trees and the target URLs.
        /// </summary>
        void browseToUrl()
        {
            var url = _view.Url;

            if (String.IsNullOrWhiteSpace(url))
            {
                _view.ShowWarningMessage();
                return;
            }
            
            try
            {
                if (url.StartsWith("http"))
                {
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                    request.Method = "HEAD";
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        _view.NavigateTo(url);
                    }
                }
                else
                {
                    try
                    {
                        FileWebRequest request = FileWebRequest.Create(url) as FileWebRequest;
                        request.Method = "HEAD";
                        FileWebResponse response = request.GetResponse() as FileWebResponse;
                        _view.NavigateTo(url);
                    }
                    catch (Exception)
                    {

                        _view.ShowRequestNotFoundMessage();
                    }
                }
            }
            catch (Exception)
            {
                _view.ShowRequestNotFoundMessage();
            }
        }

        void browserCompleted()
        {
            _view.ClearDOMTree();
            _document = new DocumentQuery(_view.GetHtmlDocument());
            var elem = _document.GetHtmlElement();
            // Build the DOM tree structure from the HTML element of the page
            var rootNode = _builder.BuildDom(elem);
            // Assign the DOM tree structure to the DOM TreeView
            _view.FillDomTree(rootNode);
            // Append the URL of the current page to the TargetURLs container
            _view.AppendTargetUrl(_view.Url);
        }
    }
}
