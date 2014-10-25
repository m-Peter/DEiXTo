using DEiXTo.Presenters;
using System;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    /// <summary>
    /// Defines the API which any concrete Form implementation
    /// must conform to.
    /// </summary>
    public interface IMainView
    {
        MainPresenter Presenter { get; set; }

        void CascadeAgents();
        void CloseAgents();
        void FloatAgents();
        bool AskUserToConfirmClosing();
    }
}
