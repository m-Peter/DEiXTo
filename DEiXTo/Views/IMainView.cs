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
        event Action CreateNewAgent;
        event Action CascadeAgentWindows;
        event Action CloseAgentWindows;
        event Action FloatAgentWindows;
        event Action<FormClosingEventArgs> WindowClosing;

        void CascadeAgents();
        void CloseAgents();
        void FloatAgents();
        bool AskUserToConfirmClosing();
    }
}
