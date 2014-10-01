using DEiXTo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Presenters
{
    public class DeixtoAgentPresenter
    {
        private readonly IDeixtoAgentView _view;

        public DeixtoAgentPresenter(IDeixtoAgentView view)
        {
            _view = view;

            _view.BrowseToUrl += _view_BrowseToUrl;
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
