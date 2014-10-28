using DEiXTo.Services;
using DEiXTo.Views;
using System;
using System.Windows.Forms;

namespace DEiXTo.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class MainPresenter : ISubscriber<EventArgs>
    {
        #region Instance Variables
        //private readonly IMainView _view;
        private readonly IViewLoader _viewLoader;
        private readonly IEventHub _eventHub;
        // count the number of childs contained in the associated View
        private int _formCounter = 0;
        #endregion

        #region Constructors
        public MainPresenter(IMainView view, IViewLoader viewLoader, IEventHub eventHub)
        {
            View = view;
            _viewLoader = viewLoader;
            _eventHub = eventHub;
            View.Presenter = this;

            eventHub.Subscribe<EventArgs>(this);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the number of DeixtoAgentWindows.
        /// </summary>
        public int FormCounter
        {
            get { return _formCounter; }
        }

        public IMainView View { get; set; }

        /// <summary>
        /// Creates a new DeixtoAgentWindow and gives it an
        /// appropriate title.
        /// </summary>
        public void CreateNewAgent()
        {
            string title = string.Format("Agent {0}", _formCounter + 1);
            _viewLoader.LoadAgentView(title, View);
            _formCounter++;
        }

        /// <summary>
        /// Cascades all the DeixtoAgentWindows.
        /// </summary>
        public void CascadeAgentWindows()
        {
            View.CascadeAgents();
        }

        /// <summary>
        /// Closes all the DeixtoAgentWindows.
        /// </summary>
        public void CloseAgentWindows()
        {
            View.CloseAgents();
            _formCounter = 0;
        }

        /// <summary>
        /// Floats all the DeixtoAgentWindows.
        /// </summary>
        public void FloatAgentWindows()
        {
            View.FloatAgents();
        }

        /// <summary>
        /// Attemps to close the view's Window, by asking the
        /// user if that's what he wants.
        /// </summary>
        public void WindowClosing(FormClosingEventArgs args)
        {
            bool confirm = View.AskUserToConfirmClosing();

            if (!confirm)
            {
                args.Cancel = true;
                return;
            }

            args.Cancel = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        public void Receive(EventArgs subject)
        {
            _formCounter--;
        }
        #endregion
    }
}
