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
        private int _formCounter = 0;

        public MainPresenter(IMainView view, IViewLoader loader)
        {
            _view = view;
            _viewLoader = loader;

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

        public int FormCounter
        {
            get { return _formCounter; }
        }

    }
}
