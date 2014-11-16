using DEiXTo.Presenters;
using System;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    public partial class MainWindow : Form, IMainView
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainPresenter Presenter { get; set; }

        public void CascadeAgents()
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        public void FloatAgents()
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.WindowState = FormWindowState.Maximized;
            }
        }

        public void CloseAgents()
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        public bool AskUserToConfirmClosing()
        {
            var result = MessageBox.Show("Are you sure you want to exit?", "DEiXTo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            return result == DialogResult.Yes;
        }

        private void newAgentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.CreateNewAgent();
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.CascadeAgentWindows();
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.CloseAgentWindows();
        }

        private void floatAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.FloatAgentWindows();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Presenter.WindowClosing(e);
        }

        private void UpdateBrowserVersionMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.UpdateBrowserVersion();
        }

        private void ResetToDefaultMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.ResetBrowserVersion();
        }
    }
}
