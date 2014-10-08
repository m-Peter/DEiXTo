using DEiXTo.Presenters;
using DEiXTo.Views;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public class WindowsViewLoader : IViewLoader
    {
        private Form _lastLoadedView;

        public void LoadMainView()
        {
            MainWindow window = new MainWindow();
            MainPresenter presenter = new MainPresenter(window, this);

            LoadView(window);
        }

        public void LoadAgentView(string title, IMainView parent)
        {
            DeixtoAgentWindow window = new DeixtoAgentWindow();
            DeixtoAgentPresenter presenter = new DeixtoAgentPresenter(window);
            
            window.Text = title;
            window.MdiParent = (Form)parent;

            LoadView(window);
        }

        public Form LastLoadedView
        {
            get { return _lastLoadedView; }
        }

        private void LoadView(Form view)
        {
            view.Show();
            _lastLoadedView = view;
        }
    }
}
