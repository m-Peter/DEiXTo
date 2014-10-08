using System;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    public partial class MainWindow : Form, IMainView
    {
        // Fires when the NewAgent menu item gets clicked
        public event Action CreateNewAgent;
        // Fires when the CascadeAgents menu item gets clicked
        public event Action CascadeAgentWindows;
        // Fires when the CloseAgents menu item gets clicked
        public event Action CloseAgentWindows;
        // Fires when the FloatAgents menu item gets clicked
        public event Action FloatAgentWindows;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CascadeAgents()
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        /// <summary>
        /// 
        /// </summary>
        public void FloatAgents()
        {
            foreach (Form childForm in this.MdiChildren)
            {
                childForm.WindowState = FormWindowState.Maximized;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CloseAgents()
        {
            foreach (Form childForm in this.MdiChildren)
            {
                childForm.Close();
            }
        }

        private void newAgentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CreateNewAgent != null)
            {
                CreateNewAgent();
            }
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CascadeAgentWindows != null)
            {
                CascadeAgentWindows();
            }
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CloseAgentWindows != null)
            {
                CloseAgentWindows();
            }
        }

        private void floatAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FloatAgentWindows != null)
            {
                FloatAgentWindows();
            }
        }
    }
}
