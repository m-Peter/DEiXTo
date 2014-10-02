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
        }

        void _view_DOMNodeClick(int index)
        {
            _styling.UnstyleElements();
            var element = _document.GetElementByIndex(index);

            _styling.Style(element);
        }

        void _view_DocumentMouseLeave(HtmlElement element)
        {
            if (!_view.HighlightModeEnabled())
            {
                return;
            }

            _styling.Unstyle(element);
        }

        void _view_DocumentMouseOver(HtmlElement element)
        {
            if (!_view.HighlightModeEnabled())
            {
                return;
            }
            _styling.UnstyleElements();
            _styling.Style(element);

            var node = _builder.TreeNodeFromElement(element);

            _view.SelectDOMNode(node);
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
            _document = new DocumentQuery(_view.GetHTMLDocument());

            var elem = _document.GetHTMLElement();
            var rootNode = _builder.BuildDom(elem);
            _view.FillDomTree(rootNode);
            _view.AppendTargetUrl(_view.Url);
        }
    }
}
