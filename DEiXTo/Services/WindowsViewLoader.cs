using DEiXTo.Presenters;
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

        public void LoadMainView()
        {
            MainWindow window = new MainWindow();
            MainPresenter presenter = new MainPresenter(window, this, EventHub.Instance);

            LoadView(window);
        }

        public void LoadAgentView(string title, IMainView parent)
        {
            DeixtoAgentWindow window = new DeixtoAgentWindow();
            DeixtoAgentScreen screen = new DeixtoAgentScreen();
            DeixtoAgentPresenter presenter = 
                new DeixtoAgentPresenter(window, this, EventHub.Instance, screen);
            
            window.Text = title;
            window.MdiParent = (Form)parent;

            LoadView(window);
        }

        public void LoadAddLabelView(TreeNode node)
        {
            AddLabelWindow window = new AddLabelWindow();
            AddLabelPresenter presenter = new AddLabelPresenter(window, node);

            window.ShowDialog();
        }

        public void LoadRegexBuilderView(TreeNode node)
        {
            RegexBuilderWindow window = new RegexBuilderWindow();
            RegexBuilderPresenter presenter =
                new RegexBuilderPresenter(window, node, EventHub.Instance);

            window.ShowDialog();
        }

        public void LoadAddSiblingOrderView(TreeNode node)
        {
            AddSiblingOrderWindow window = new AddSiblingOrderWindow();
            AddSiblingOrderPresenter presenter =
                new AddSiblingOrderPresenter(window, node);

            window.ShowDialog();
        }

        public void LoadAddAttributeConstraintView(TreeNode node)
        {
            AddAttributeConstraintWindow window = new AddAttributeConstraintWindow();
            AddAttributeConstraintPresenter presenter =
                new AddAttributeConstraintPresenter(window, node);

            window.ShowDialog();
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
