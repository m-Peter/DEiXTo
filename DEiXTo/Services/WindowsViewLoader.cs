using DEiXTo.Presenters;
using DEiXTo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public class WindowsViewLoader : IViewLoader
    {
        private Form lastLoadedView;

        public void LoadMainView()
        {
            MainWindow window = new MainWindow();
            MainPresenter presenter = new MainPresenter(window, this);

            LoadView(window);
        }

        public void LoadAgentView(string title, IMainView parent)
        {
            DeixtoAgentWindow window = new DeixtoAgentWindow();
            window.Text = title;
            window.MdiParent = (Form)parent;

            LoadView(window);
        }

        private void LoadView(Form view)
        {
            view.Show();
            lastLoadedView = view;
        }

        public Form LastLoadedView
        {
            get { return lastLoadedView; }
        }
    }
}
