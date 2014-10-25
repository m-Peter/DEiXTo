using DEiXTo.Presenters;
using System;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    public interface IRegexBuilderView
    {
        RegexBuilderPresenter Presenter { get; set; }

        string RegexText { get; set; }

        void ShowInvalidRegexMessage();
        void Exit();
    }
}
