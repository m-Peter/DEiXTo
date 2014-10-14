using DEiXTo.Services;
using DEiXTo.Views;

namespace DEiXTo.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class MainPresenter
    {
        private readonly IMainView _view;
        private readonly IViewLoader _viewLoader;
        // count the number of childs contained in the associated View
        private int _formCounter = 0;

        public MainPresenter(IMainView view, IViewLoader viewLoader)
        {
            _view = view;
            _viewLoader = viewLoader;

            // ATTACH THE EVENTS OF THE VIEW TO LOCAL METHODS
            _view.CreateNewAgent += createNewAgent;
            _view.CascadeAgentWindows += cascadeAgentWindows;
            _view.CloseAgentWindows += closeAgentWindows;
            _view.FloatAgentWindows += floatAgentWindows;
        }

        /// <summary>
        /// 
        /// </summary>
        void floatAgentWindows()
        {
            _view.FloatAgents();
        }

        /// <summary>
        /// 
        /// </summary>
        void closeAgentWindows()
        {
            _view.CloseAgents();
            _formCounter = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        void cascadeAgentWindows()
        {
            _view.CascadeAgents();
        }

        /// <summary>
        /// 
        /// </summary>
        void createNewAgent()
        {
            string title = string.Format("Agent {0}", _formCounter + 1);
            _viewLoader.LoadAgentView(title, _view);
            _formCounter++;
        }

    }
}
