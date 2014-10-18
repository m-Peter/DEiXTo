﻿using DEiXTo.Services;
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
        private readonly IMainView _view;
        private readonly IViewLoader _viewLoader;
        // count the number of childs contained in the associated View
        private int _formCounter = 0;

        public MainPresenter(IMainView view, IViewLoader viewLoader)
        {
            _view = view;
            _viewLoader = viewLoader;
            EventHub eventHub = EventHub.Instance;

            // ATTACH THE EVENTS OF THE VIEW TO LOCAL METHODS
            _view.CreateNewAgent += createNewAgent;
            _view.CascadeAgentWindows += cascadeAgentWindows;
            _view.CloseAgentWindows += closeAgentWindows;
            _view.FloatAgentWindows += floatAgentWindows;
            _view.WindowClosing += windowClosing;

            eventHub.Subscribe<EventArgs>(this);
        }

        /// <summary>
        /// Attemps to close the view's Window, by asking the
        /// user if that's what he wants.
        /// </summary>
        void windowClosing(FormClosingEventArgs args)
        {
            bool answer = _view.AskUserToConfirmClosing();

            if (!answer)
            {
                args.Cancel = true;
                return;
            }

            args.Cancel = false;
        }

        /// <summary>
        /// Floats all the DeixtoAgentWindows.
        /// </summary>
        void floatAgentWindows()
        {
            _view.FloatAgents();
        }

        /// <summary>
        /// Closes all the DeixtoAgentWindows.
        /// </summary>
        void closeAgentWindows()
        {
            _view.CloseAgents();
            _formCounter = 0;
        }

        /// <summary>
        /// Cascades all the DeixtoAgentWindows.
        /// </summary>
        void cascadeAgentWindows()
        {
            _view.CascadeAgents();
        }

        /// <summary>
        /// Creates a new DeixtoAgentWindow and gives it an
        /// appropriate title.
        /// </summary>
        void createNewAgent()
        {
            string title = string.Format("Agent {0}", _formCounter + 1);
            _viewLoader.LoadAgentView(title, _view);
            _formCounter++;
        }

        /// <summary>
        /// Returns the number of DeixtoAgentWindows.
        /// </summary>
        public int FormCounter
        {
            get { return _formCounter; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        public void Receive(EventArgs subject)
        {
            _formCounter--;
        }
    }
}
