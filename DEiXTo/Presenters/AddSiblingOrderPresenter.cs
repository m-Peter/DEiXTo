using DEiXTo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Presenters
{
    public class AddSiblingOrderPresenter
    {
        private IAddSiblingOrderView _view;

        public AddSiblingOrderPresenter(IAddSiblingOrderView view)
        {
            _view = view;
        }
    }
}
