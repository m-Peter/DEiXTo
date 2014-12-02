using DEiXTo.Services;
using DEiXTo.Views;
using System;
using System.Windows.Forms;

namespace DEiXTo.Presenters
{
    public class MainWindowPresenter : ISubscriber<DeixtoAgentClosed>, IMainWindowPresenter
    {
        private readonly IViewLoader _viewLoader;
        private readonly IEventHub _eventHub;
        // count the number of childs contained in the associated View
        private int _formCounter = 0;
        private readonly IBrowserVersionManager _browserManager;

        public MainWindowPresenter(IMainView view, IViewLoader viewLoader, IEventHub eventHub)
        {
            View = view;
            _viewLoader = viewLoader;
            _eventHub = eventHub;
            _browserManager = new BrowserVersionManager();
            View.Presenter = this;

            eventHub.Subscribe<DeixtoAgentClosed>(this);
        }

        public int FormCounter
        {
            get { return _formCounter; }
        }

        public IMainView View { get; set; }

        public void CreateNewAgent()
        {
            string title = string.Format("Agent {0}", _formCounter + 1);
            _viewLoader.LoadAgentView(title, View);
            _formCounter++;
        }

        public void CascadeAgentWindows()
        {
            View.CascadeAgents();
        }

        public void CloseAgentWindows()
        {
            View.CloseAgents();
            _formCounter = 0;
        }

        public void FloatAgentWindows()
        {
            View.FloatAgents();
        }

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

        public void UpdateBrowserVersion()
        {
            _browserManager.UpdateBrowserVersion();
        }

        public void ResetBrowserVersion()
        {
            _browserManager.ResetBrowserVersion();
        }

        public void Receive(DeixtoAgentClosed subject)
        {
            _formCounter--;
        }
    }
}
