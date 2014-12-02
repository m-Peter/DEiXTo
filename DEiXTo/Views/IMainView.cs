using DEiXTo.Presenters;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    public interface IMainView
    {
        IMainWindowPresenter Presenter { get; set; }

        void CascadeAgents();
        void CloseAgents();
        void FloatAgents();
        bool AskUserToConfirmClosing();
    }
}
