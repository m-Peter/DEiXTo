using DEiXTo.Models;
using DEiXTo.Presenters;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    public interface IRegexBuilderView
    {
        RegexBuilderPresenter Presenter { get; set; }

        string RegexText { get; set; }
        string InputText { get; set; }

        void ShowInvalidRegexMessage();
        void Exit();
    }
}
