using System;

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

        void CascadeAgents();
        void CloseAgents();
        void FloatAgents();
    }
}
