﻿using DEiXTo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Presenters
{
    public class AddLabelPresenter
    {
        private IAddLabelView _view;

        public AddLabelPresenter(IAddLabelView view)
        {
            _view = view;

            _view.AddLabel += addLabel;
        }

        void addLabel()
        {
            string label = _view.GetLabelText();

            if (String.IsNullOrWhiteSpace(label))
            {
                _view.ShowInvalidLabelMessage();
                _view.Exit();
            }
        }
    }
}
