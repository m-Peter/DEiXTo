using DEiXTo.Presenters;
using System;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    public interface IAddLabelView
    {
        AddLabelPresenter Presenter { get; set; }

        string LabelText { get; set; }

        void ShowInvalidLabelMessage();
        void Exit();
    }
}
