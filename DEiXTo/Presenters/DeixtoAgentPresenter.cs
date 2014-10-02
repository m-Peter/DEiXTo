using DEiXTo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Presenters
{
    public class DeixtoAgentPresenter
    {
        private readonly IDeixtoAgentView _view;

        public DeixtoAgentPresenter(IDeixtoAgentView view)
        {
            _view = view;

            _view.BrowseToUrl += _view_BrowseToUrl;
            _view.KeyDownPress += _view_KeyDownPress;
            _view.AutoFillChanged += _view_AutoFillChanged;
            _view.CrawlingChanged += _view_CrawlingChanged;
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
    }
}
