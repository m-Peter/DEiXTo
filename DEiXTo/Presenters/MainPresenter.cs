using DEiXTo.Services;
using DEiXTo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Presenters
{
    public class MainPresenter
    {
        private readonly IMainView _view;
        private readonly IViewLoader _viewLoader;
        // count the number of childs the associated View contains
        private int _formCounter = 0;

        public MainPresenter(IMainView view, IViewLoader viewLoader)
        {
            _view = view;
            _viewLoader = viewLoader;

            // ATTACH THE EVENTS OF THE VIEW TO LOCAL METHODS
            _view.NewAgent += _view_NewAgent;
            _view.CascadeAgentWindows += _view_CascadeAgentWindows;
            _view.CloseAgentWindows += _view_CloseAgentWindows;
            _view.FloatAgentWindows += _view_FloatAgentWindows;
        }

        void _view_FloatAgentWindows()
        {
            _view.FloatAgents();
        }

        void _view_CloseAgentWindows()
        {
            _view.CloseAgents();
            _formCounter = 0;
        }

        void _view_CascadeAgentWindows()
        {
            _view.CascadeAgents();
        }

        void _view_NewAgent()
        {
            string title = string.Format("Agent {0}", _formCounter + 1);
            _viewLoader.LoadAgentView(title, _view);
            _formCounter++;
        }

    }
}
