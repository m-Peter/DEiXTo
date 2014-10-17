﻿using DEiXTo.Presenters;
using DEiXTo.Views;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class WindowsViewLoader : IViewLoader
    {
        private Form _lastLoadedView;

        /// <summary>
        /// 
        /// </summary>
        public void LoadMainView()
        {
            MainWindow window = new MainWindow();
            MainPresenter presenter = new MainPresenter(window, this);

            LoadView(window);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="parent"></param>
        public void LoadAgentView(string title, IMainView parent)
        {
            DeixtoAgentWindow window = new DeixtoAgentWindow();
            DeixtoAgentPresenter presenter = new DeixtoAgentPresenter(window);
            
            window.Text = title;
            window.MdiParent = (Form)parent;

            LoadView(window);
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadAddLabelView()
        {
            AddLabelWindow window = new AddLabelWindow();
            AddLabelPresenter presenter = new AddLabelPresenter(window);

            window.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadRegexBuilderView()
        {
            RegexBuilderWindow window = new RegexBuilderWindow();

            window.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        public Form LastLoadedView
        {
            get { return _lastLoadedView; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        private void LoadView(Form view)
        {
            view.Show();
            _lastLoadedView = view;
        }
    }
}
