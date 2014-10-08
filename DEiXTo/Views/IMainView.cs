using System;

namespace DEiXTo.Views
{
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
